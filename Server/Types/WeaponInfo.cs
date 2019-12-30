using System;
using System.Collections.Generic;
using System.Text;

using OmniTanks.Components;

namespace Server.Types
{
    public class WeaponInfo
    {
        public int ID = -1;
        public virtual string Name { get; } = string.Empty;
        public virtual string DisplayName { get; } = string.Empty;
        public enum Attachments
        {
            None,
            Main,
            Aux,
            Ether,
        }

        public virtual Dictionary<ChasisClasses, Attachments> ChassisAttachments { get; } = new Dictionary<ChasisClasses, Attachments>();
        public virtual Dictionary<ChasisClasses, string> GraphicEntities { get; } = new Dictionary<ChasisClasses, string>();

        public virtual float ChargePerShot { get; } = 0;
        public virtual int AmmoPerShot { get; } = 0;
        public virtual int MaxAmmo { get; } = 0;

        public virtual float MinReloadTime { get; } = 0;

        public virtual string ShotClassName { get; } = string.Empty;
    }
}
