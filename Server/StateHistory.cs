using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Urho;

namespace Server
{
    public static class StateHistory
    {
        public const int TrackedInputCount = 4;

        public const int StateHistorySize = 200;
        public const int InitalItemCount = 100;

        public class StateData
        {
            public int StepIndex = -1;
            public int PrevStepIndex = -1;
            public int NextStepIndex = -1;

            public UInt64 StateID = UInt64.MinValue;
            public double TimeStamp = double.MinValue;

            public class ItemState
            {
                public bool Valid = false;
                public Vector3 Postion = new Vector3(0, 0, 0);
                public Quaternion Orientation = new Quaternion();

                public UInt64 ItemGUID = UInt64.MaxValue;

                public Vector3 Velocity = new Vector3(0, 0, 0);
                public Int16[] Inputs = new short[TrackedInputCount];
            }

            public ItemState[] TrackedItems = new ItemState[0];
        }

        private static StateData[] StateList = new StateData[StateHistorySize];

        private static int CurrentStateIndex = 0;
        private static UInt64 CurrentStateID = UInt64.MinValue;

        private static Dictionary<UInt64, int> GUIDIndexMap = new Dictionary<ulong, int>();
        private static List<int> DeadIndexes = new List<int>();

        static StateHistory()
        {
            for (int i = 0; i < StateHistorySize; i++)
            {
                StateList[i] = new StateData();
                StateList[i].StepIndex = i;
                StateList[i].PrevStepIndex = i - 1;
                StateList[i].NextStepIndex = i + 1;
                StateList[i].TrackedItems = new StateData.ItemState[InitalItemCount];
                for (int j = 0; j < InitalItemCount; j++)
                {
                    StateList[i].TrackedItems[j] = new StateData.ItemState();
                }
            }
            StateList[0].PrevStepIndex = StateList.Length - 1;
            StateList[StateList[0].PrevStepIndex].NextStepIndex = 0;

            for (int j = 0; j < InitalItemCount; j++)
                DeadIndexes.Add(j);
        }

        public static void AddTrackedItem(UInt64 ItemGUID)
        {
            if (GUIDIndexMap.ContainsKey(ItemGUID))
                return;

            if (DeadIndexes.Count == 0)
            {
                int lastIndex = StateList[0].TrackedItems.Length;

                for (int i = 0; i < StateHistorySize; i++)
                {
                    Array.Resize(ref StateList[i].TrackedItems, lastIndex + InitalItemCount);
                    for (int j = lastIndex; j < StateList[i].TrackedItems.Length; j++)
                    {
                        StateList[i].TrackedItems[j] = new StateData.ItemState();
                    }
                }
                for (int j = lastIndex; j < StateList[0].TrackedItems.Length; j++)
                    DeadIndexes.Add(j);
            }

            int index = DeadIndexes[0];
            DeadIndexes.RemoveAt(0);

            GUIDIndexMap.Add(ItemGUID, index);
            for (int i = 0; i < StateHistorySize; i++)
            {
                StateList[i].TrackedItems[index].ItemGUID = ItemGUID;
            }
        }

        public static void RemoveTrackedItem(UInt64 ItemGUID)
        {
            int index = Array.FindIndex(StateList[0].TrackedItems, x => x.ItemGUID == ItemGUID);
            if (index == -1)
                return;

            GUIDIndexMap.Remove(ItemGUID);
            DeadIndexes.Insert(index, 0);
        }

        public delegate void GetItemStateCB(UInt64 GUID, StateData.ItemState state, double timestamp);
        public delegate void InterpolateItemStateCB(UInt64 GUID, StateData.ItemState prevState, StateData.ItemState state, double prevTimestamp, double timestamp);
       
        public static GetItemStateCB UpdateItemState = null;
        public static InterpolateItemStateCB InterpolateItemState = null;

        public static UInt64 IncrementState(double timestamp)
        {
            CurrentStateIndex++;
            if (CurrentStateIndex >= StateHistorySize)
                CurrentStateIndex = 0;

            if (CurrentStateID == UInt64.MaxValue)
                CurrentStateID = 0;
            else
                CurrentStateID++;

            StateData state = StateList[CurrentStateIndex];
            state.StateID = CurrentStateID;
            state.TimeStamp = timestamp;

            for (int i = 0; i < state.TrackedItems.Length; i++)
            {
                if (DeadIndexes.Contains(i))
                    continue;

                state.TrackedItems[i].Valid = true;
                UpdateItemState?.Invoke(state.TrackedItems[i].ItemGUID, state.TrackedItems[i], timestamp);
            }

            return CurrentStateID;
        }

        public static StateData GetStateBuyIndex(int index)
        {
            return StateList[index % StateHistorySize];
        }

        public static StateData GetPevState(StateData state)
        {
            return StateList[state.PrevStepIndex];
        }

        public static StateData GetNextState(StateData state)
        {
            return StateList[state.NextStepIndex];
        }

        public static StateData.ItemState GetItemState(StateData state, UInt64 GUID)
        {
            if (!GUIDIndexMap.ContainsKey(GUID))
                return null;

            return state.TrackedItems[GUIDIndexMap[GUID]];
        }

        public static StateData StepForTime(double timestamp)
        {
            StateData stepToTest = StateList[CurrentStateIndex];
            for (int i = 0; i < StateHistorySize; i++)
            {
                if (stepToTest.TimeStamp == timestamp)
                    return stepToTest;

                StateData prevStep = GetPevState(stepToTest);
                if (prevStep.TimeStamp < timestamp)
                    return stepToTest;

                stepToTest = prevStep;
            }

            return null;
        }

        public static void InsertItemUpdate(UInt64 GUID, double insertTimeStamp, StateData.ItemState state)
        {
            StateData insertStep = StepForTime(insertTimeStamp);
            if (insertStep == null)
                return;

            if (insertStep.TimeStamp > insertTimeStamp)
            {
                // we have to do some interpolation, our update wasn't on a step, so interp from the delta
                InterpolateItemState?.Invoke(GUID, state, GetItemState(insertStep,GUID), insertTimeStamp, insertStep.TimeStamp);
            }

            while(insertStep.NextStepIndex != CurrentStateIndex)
        }
    }
}
