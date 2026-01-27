using System;

namespace Game.Core.Entity.Weapon
{
    public abstract class WeaponObject : ItemObject
    {
        public override Type ControllerType => typeof(WeaponController);
    }
}