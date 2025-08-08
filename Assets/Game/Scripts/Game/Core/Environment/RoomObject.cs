using Moduls.Light;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.Environment
{
    public sealed class RoomObject : MonoBehaviour
    {
        public bool IsCeilLampsEnabled => CeilLamps.FirstOrDefault()?.Light.enabled ?? false;
        
        [ field: SerializeField ] public List< Lamp > CeilLamps { get; private set; }
        
        public void EnableCeilLamps( bool trigger )
        {
            for ( int i = 0; i < CeilLamps.Count; i++ )
            {
                CeilLamps[ i ].Enable( trigger );
            }
        }
    }
}