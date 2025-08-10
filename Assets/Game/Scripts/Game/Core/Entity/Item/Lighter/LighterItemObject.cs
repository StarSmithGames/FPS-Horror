using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class LighterItemObject : ItemObject
    {
        [ SerializeField ] private Light _light;

        public void EnableLight( bool trigger )
        {
            _light.enabled = trigger;
        }
    }
}