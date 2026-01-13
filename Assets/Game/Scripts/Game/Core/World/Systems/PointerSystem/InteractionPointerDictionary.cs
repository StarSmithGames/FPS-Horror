using Game.Core.Entity;
using System.Collections.Generic;

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

        public bool Contains( InteractableObject target ) => _pointers.ContainsKey( target );
        
        public bool IsPointerShowing( InteractableObject target )
        {
            if ( Contains( target ) )
            {
                return Get( target ).IsShowing;
            }

            return false;
        }

        public void ShowPointer( InteractableObject target )
        {
            var pointer = Get( target );

            if ( pointer.IsShowing ) return;
            
            pointer.transform.position = target.PointerPosition;
            pointer.Show();
        }
        
        public void HidePointer( InteractableObject target )
        {
            var pointer = Get( target );

            if ( !pointer.IsShowing ) return;
            
            pointer.Hide();
        }
    }
}