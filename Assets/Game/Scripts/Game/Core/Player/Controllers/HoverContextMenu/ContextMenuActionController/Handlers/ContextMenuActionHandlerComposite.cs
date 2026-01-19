using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionHandlerComposite
    {
        public event Action OnCompleted;

        private readonly List< ContextMenuActionHandler > _handlers;
        
        public ContextMenuActionHandlerComposite( List< ContextMenuActionHandler > handlers )
        {
            _handlers = handlers ?? throw new ArgumentNullException( nameof(handlers) );
        }

        public void Initialize( ObservableObject target )
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].Initialize( target );
                _handlers[ i ].OnCompleted += ActionCompletedHandler;
            }
        }

        public void Dispose()
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].OnCompleted -= ActionCompletedHandler;
                _handlers[ i ].Dispose();
            }
        }

        public void SetOptions( List< UIInfoButton > options  )
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].Enable( options[ i ] );
            }
        }
        
        private void ActionCompletedHandler( ContextMenuActionHandler actionHandler )
        {
            OnCompleted?.Invoke();
        }
        
        public List< ContextMenuOperation > GetContextMenuOptions() => _handlers.Select( ( x ) => x.ContextMenuOperation ).ToList();
    }
}