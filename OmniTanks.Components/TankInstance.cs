using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Urho;

namespace OmniTanks.Components
{
    public class TankInstance : LogicComponent
    {
        public int GroupID = -1;
        public ChasisClasses ChasisClass = ChasisClasses.Medium;
        public CosmeticsInfo Cosmetics = new CosmeticsInfo();

        public int PrimaryWeaponID = -1;
        public int AuxiliaryWeaponID = -1;
        public int PassiveAccessoryID = -1;
        public int ActiveAccessoryID = -1;

        public double ChargeLevel = 0;
        public double ArmorLevel = 0;
        public int Ammunition = 0;

        public float PrimaryReload = 0;
        public float AuxReload = 0;
    }
}
