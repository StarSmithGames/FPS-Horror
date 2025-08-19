using Game.Core.Player;
using Game.Core.World.InteractionSystem;
using Game.Systems.InventorySystem;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Core.Entity
{
    public sealed class FuseboxPuzzle : PuzzleObject
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

            _door.OnChanged += DoorChangedHandler;
            DoorChangedHandler();
        }

        public override void Dispose()
        {
            _door.OnChanged -= DoorChangedHandler;

            base.Dispose();
        }

        public override void Interact( IInteractor interactor )
        {
            if ( interactor is not PlayerController player ) return;

            var controller = player.ServiceLocator.GetAs< PlayerInventoryController >();
            if ( controller.Inventory.ContainsItem( ItemDatabase.FUSE ) )
            {
                controller.Inventory.RemoveItem( ItemDatabase.FUSE );

                for ( int i = 0; i < Slots.Count; i++ )
                {
                    Slots[ i ].IsInserted = true;
                }
                RefreshSlots();
            }
            
            EnableCollider( _door.IsOpen && !IsFusesConnected );
        }

        private void RefreshSlots()
        {
            for ( int i = 0; i < Slots.Count; i++ )
            {
                var slot = Slots[ i ];
                
                if ( slot.IsInserted )
                {
                    slot.Light.material = _lightGreen;
                    if ( slot.Nest.childCount == 0 )
                    {
                        var fuse = GameObject.Instantiate( _fusePrefab, slot.Nest );
                        fuse.transform.localPosition = Vector3.zero;
                        fuse.transform.localScale = Vector3.one;
                        fuse.transform.localRotation = Quaternion.identity;
                    }
                }
                else
                {
                    slot.Light.material = _lightRed;
                    slot.Nest.DestroyChildren();
                }
            }
        }

        private void DoorChangedHandler()
        {
            Debug.LogError( _door.IsOpen + " " + IsFusesConnected );
            
            EnableCollider( _door.IsOpen && !IsFusesConnected );
        }
    }
}