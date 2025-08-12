using PuzzlescapeGames.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class FuseboxPuzzle : ObservableObject
    {
        [ field: SerializeField ] public List< FuseboxSlot > Slots { get; private set; } = new();
        [ Space ]
        [ SerializeField ] private OpenableObject _door;
        [ SerializeField ] private Material _lightGreen;
        [ SerializeField ] private Material _lightRed;
        [ SerializeField ] private Fuse _fusePrefab;

        public bool IsFusesConnected => Slots.All( ( x ) => x.IsInserted );

        private void Start()
        {
            RefreshSlots();
        }
        
        private void RefreshSlots()
        {
            for ( int i = 0; i < Slots.Count; i++ )
            {
                var slot = Slots[ i ];
                
                if ( slot.IsInserted )
                {
                    slot.Light.material = _lightGreen;
                    slot.Nest.DestroyChildren();
                    var fuse = GameObject.Instantiate( _fusePrefab, slot.Nest );
                    fuse.transform.localPosition = Vector3.zero;
                    fuse.transform.localScale = Vector3.one;
                    fuse.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    slot.Light.material = _lightRed;
                    slot.Nest.DestroyChildren();
                }
            }
        }
    }
}