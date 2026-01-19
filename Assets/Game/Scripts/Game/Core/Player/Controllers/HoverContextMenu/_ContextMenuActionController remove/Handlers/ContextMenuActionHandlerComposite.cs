using Game.Core.Entity;
using Game.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionHandlerComposite
    {
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
            }
        }

        public void Dispose()
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].Dispose();
            }
        }

        public void SetOptions( List< UIInfoButton > options  )
        {
            for ( int i = 0; i < _handlers.Count; i++ )
            {
                _handlers[ i ].Set( options[ i ] );
            }
        }
        
        public List< ContextMenuOperation > GetContextMenuOptions() => _handlers.Select( ( x ) => x.ContextMenuOperation ).ToList();
    }
}