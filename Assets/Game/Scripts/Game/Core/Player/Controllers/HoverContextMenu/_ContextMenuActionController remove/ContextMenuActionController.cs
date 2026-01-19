using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionController
    {
        private bool _isShowingInformer;
        
        private UITargetInformer _targetInformer;
        private ContextMenuActionHandlerComposite _contextMenuActionHandlerComposite;
        
        private readonly ContextMenuActionFactory _contextMenuActionFactory;
        private readonly ILocalizationSystem _localizationSystem;

        public ContextMenuActionController(
            ContextMenuActionFactory contextMenuActionFactory,
            ILocalizationSystem localizationSystem
            )
        {
            _contextMenuActionFactory = contextMenuActionFactory ?? throw new ArgumentNullException( nameof(contextMenuActionFactory) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize( UITargetInformer targetInformer )
        {
            _targetInformer = targetInformer;
        }

        public void SetToDynamic( OpenCloseObject dynamic )
        {
            _targetInformer.Name.text = dynamic.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( dynamic.NameId );
            _contextMenuActionHandlerComposite = _contextMenuActionFactory.GetOrCreateOpenCloseHandler();
        }

        public void SetToItem( ItemObject item )
        {
            _targetInformer.Name.text = item.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( item.NameId );
            if ( item is Note )
            {
                _contextMenuActionHandlerComposite = _contextMenuActionFactory.GetOrCreateItemNoteHandler();
            }
                
            _contextMenuActionHandlerComposite = _contextMenuActionFactory.GetOrCreateItemHandler();
        }

        public void SetToPuzzle( PuzzleObject puzzle )
        {
            _targetInformer.Name.text = "Puzzle";
            _contextMenuActionHandlerComposite = _contextMenuActionFactory.GetOrCreatePuzzleHandler();
        }

        public void CurrentObservableChangedHandler( ObservableObject observable )
        {
            _contextMenuActionHandlerComposite?.Dispose();
            
            for ( int i = 0; i < _targetInformer.Options.Count; i++ )
            {
                _targetInformer.Options[ i ].gameObject.SetActive( false );
                _targetInformer.Options[ i ].SetFillAmount( 0f );
            }

            if ( observable == null )
            {
                _contextMenuActionHandlerComposite = null;
                
                if ( _isShowingInformer )
                {
                    _isShowingInformer = false;
                    _targetInformer.Hide();
                }
                
                return;
            }
            
            if ( _contextMenuActionHandlerComposite != null )
            {
                _contextMenuActionHandlerComposite.Initialize( observable );
                _contextMenuActionHandlerComposite.SetOptions( GetOptions( _contextMenuActionHandlerComposite.GetContextMenuOptions() ) );
            }
            
            if ( !_isShowingInformer )
            {
                _isShowingInformer = true;
                _targetInformer.Show();
            }

            List< UIInfoButton > GetOptions( List< ContextMenuOperation > options )
            {
                List< UIInfoButton > result = new( options.Count );
                for ( int i = 0; i < options.Count; i++ )
                {
                    var option = options[ i ];
                    var view = _targetInformer.Options[ i ];
                       
                    view.gameObject.SetActive( true );
                    view.Set( option.Key, option.Name );
                        
                    result.Add( view );
                }

                return result;
            }
        }
    }
}