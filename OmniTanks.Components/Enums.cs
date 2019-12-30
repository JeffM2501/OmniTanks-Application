using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniTanks.Components
{
    public enum ChasisClasses
    {
        Light,
        Medium,
        Heavy
    }

    public enum WeaponTypes
    {
        Blaster,
        Cannon,
        Laser,
        MachineGun,
        ChainGun,
        TargetDesignator,
        Shotgun,
        Mortar,
        Rockets,
        RepairBeam,
        RepairField,
        Grenade,
        HeavyGrenade,
        Missiles,
        DirectionalShield,
    }

    public enum AccessoryTypes
    {
        ImprovedMotors,
        LargerCapacitor,
        ImprovedGenerator,
        ImprovedArmor,
        ShieldProjector,
        AmmoRack,
        TurboBoost,
        JumpJets,
        EnhancedSensors,
        Scavenger,
        Zoom,
        Jamming,
        DroneTurret,
    }

    public enum ShotTypes
    {
        Bolt,
        Beam,
        Shell,
        Hitscan,
        BoltBurst,
        RepairBeam,
        RepairField,
        Rocket,
        Designator,
    }
}
