using Game.Core.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.World.PointerSystem
{
    public sealed class InteractionPointerDictionary
    {
        public Dictionary< InteractableObject, InteractionPointer > Pointers => _pointers;
        private readonly Dictionary< InteractableObject, InteractionPointer > _pointers = new();

        public void Clear()
        {
            if ( _pointers.Count == 0 ) return;
            
            foreach ( var pointer in _pointers )
            {
                pointer.Key.OnColliderChanged -= InteractableColliderChangedHandler;
                if ( Contains( pointer.Key ) )
                {
                    HidePointer( pointer.Key );
                }
            }
            _pointers.Clear();
        }
        
        public void TryAdd( InteractableObject target, InteractionPointer pointer )
        {
            target.OnColliderChanged += InteractableColliderChangedHandler;
            _pointers.TryAdd( target, pointer );
        }

        public void TryRemove( InteractableObject target )
        {
            target.OnColliderChanged -= InteractableColliderChangedHandler;
            if ( Contains( target ) )
            {
                HidePointer( target );
            }
            _pointers.Remove( target );
        }

        public InteractionPointer Get( InteractableObject target ) => _pointers[ target ];

        public void TryRemoveSubtractions( List< InteractableObject > targets )
        {
            List< InteractableObject > removes = new();
            foreach ( var pointer in _pointers )
            {
                if ( !targets.Contains( pointer.Key ) )
                {
                    removes.Add( pointer.Key );
                }
            }

            for ( int i = 0; i < removes.Count; i++ )
            {
                TryRemove( removes[ i ] );
            }
        }

        public bool Contains( InteractableObject target ) => _pointers.ContainsKey( target );
        
        public bool IsShowing( InteractableObject target )
        {
            if ( Contains( target ) )
            {
                return Get( target ).IsShowing;
            }

            return false;
        }

        public void HidePointer( InteractableObject target )
        {
            var pointer = Get( target );

            if ( !pointer.IsShowing ) return;
            
            pointer.Hide( pointer.StopLookAt );
        }
        
        private void InteractableColliderChangedHandler( ObservableObject target )
        {
            if ( target.IsCollidersEnabled ) return;
            
            target.OnColliderChanged -= InteractableColliderChangedHandler;
            TryRemove( (InteractableObject)target );
        }
    }
}