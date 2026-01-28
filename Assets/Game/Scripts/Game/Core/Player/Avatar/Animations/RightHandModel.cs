using Game.Core.Entity;
using UnityEngine;

namespace Game.Core.Player.Animations
{
    public sealed class RightHandModel
    {
        public Transform Root;
        public Transform Center;
        
        public ItemObject CurrentItem;

        public Vector3 BasePos;
        public Quaternion BaseRot;
    }
}