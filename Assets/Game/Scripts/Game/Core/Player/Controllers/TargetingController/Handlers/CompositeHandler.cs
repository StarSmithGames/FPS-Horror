using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class CompositeHandler
    {
        public event Action OnCompleted;
        
        private readonly List< ActionHandler > _handlers;
        
        public CompositeHandler( List< ActionHandler > handlers )
        {
            _handlers = handlers ?? throw new ArgumentNullException( nameof(handlers) );
        }

        public void Initialize( IObservable target )
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].Initialize( target );
            }
        }

        public void Enable( List< UIInfoButton > options )
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].OnCompleted += ActionCompletedHandler;
                _handlers[ i ].Enable( options[ i ] );
            }
        }

        public void Disable()
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].OnCompleted -= ActionCompletedHandler;
                _handlers[ i ].Disable();
            }
        }

        private void ActionCompletedHandler( ActionHandler actionHandler )
        {
            OnCompleted?.Invoke();
        }
        
        public List< ContextMenuOperation > GetContextMenuOptions()
        {
            return _handlers.Select( ( x ) => x.ContextMenuOperation ).ToList();
        }
    }
}