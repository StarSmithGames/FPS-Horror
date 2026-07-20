using Game.Core.Player;
using Game.Core.World.InteractionSystem;
using Game.Core.World.InventorySystem;
using Game.StoryFlow;
using Game.StoryFlow.Introduce;
using PuzzlescapeGames.Extensions;
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

        private StoryManager _storyManager;
        
        [ Inject ]
        private void Construct( StoryManager storyManager )
        {
            _storyManager = storyManager;
        }
        
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
            if ( Slots.All( ( x ) => x.IsInserted ) ) return;
            
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

                ( (IntroduceStoryController)_storyManager.CurrentStory ).LightUp();
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
                        fuse.EnableCollider( false );
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
            EnableCollider( _door.IsOpen && !IsFusesConnected );
        }
    }
}