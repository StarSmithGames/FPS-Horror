using Game.Core.Entity;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.World.PointerSystem
{
    public sealed class InteractionPointerDictionary
    {
        private readonly Dictionary< InteractableObject, InteractionPointer > _pointers = new();

        public void TryAdd( InteractableObject target, InteractionPointer pointer )
        {
            _pointers.TryAdd( target, pointer );
        }

        public void TryRemove( InteractableObject target )
        {
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
        
        public bool IsPointerShowing( InteractableObject target )
        {
            if ( Contains( target ) )
            {
                return Get( target ).IsShowing;
            }

            return false;
        }

        public void ShowPointer( InteractableObject target, Transform lookAt )
        {
            var pointer = Get( target );

            if ( pointer.IsShowing ) return;
            
            pointer.StartLookAt( lookAt );
            pointer.Show( target );
        }
        
        public void HidePointer( InteractableObject target )
        {
            var pointer = Get( target );

            if ( !pointer.IsShowing ) return;
            
            pointer.Hide( pointer.StopLookAt );
        }
    }
}