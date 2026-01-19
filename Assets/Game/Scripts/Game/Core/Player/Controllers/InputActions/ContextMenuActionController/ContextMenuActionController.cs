using Game.Core.Entity;
using Game.Core.UI;
using Game.Core.UI.GameScreen;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class ContextMenuActionController
    {
        public event Action OnCompleted;
        
        private bool _isShowingInformer;
        
        private UITargetInformer _targetInformer;
        private World.InteractionSystem.IObservable _currentObservable;
        private ContextHandlerComposite _contextHandlerComposite;
        
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
            _contextHandlerComposite = _contextMenuActionFactory.GetOrCreateOpenCloseHandler();
        }

        public void SetToItem( ItemObject item )
        {
            _targetInformer.Name.text = item.NameId.IsEmpty() ? string.Empty : _localizationSystem.Translate( item.NameId );
            if ( item is Note )
            {
                _contextHandlerComposite = _contextMenuActionFactory.GetOrCreateItemNoteHandler();
            }
                
            _contextHandlerComposite = _contextMenuActionFactory.GetOrCreateItemHandler();
        }

        public void SetToPuzzle( PuzzleObject puzzle )
        {
            _targetInformer.Name.text = "Puzzle";
            _contextHandlerComposite = _contextMenuActionFactory.GetOrCreatePuzzleHandler();
        }

        public void CurrentObservableChangedHandler( World.InteractionSystem.IObservable observable )
        {
            _currentObservable = observable;
            
            if ( _contextHandlerComposite != null )
            {
                _contextHandlerComposite.Dispose();
                _contextHandlerComposite.OnCompleted -= ContextCompleted;
            }
            
            for ( int i = 0; i < _targetInformer.Options.Count; i++ )
            {
                _targetInformer.Options[ i ].gameObject.SetActive( false );
                _targetInformer.Options[ i ].SetFillAmount( 0f );
            }

            if ( _currentObservable == null )
            {
                _contextHandlerComposite = null;
                
                if ( _isShowingInformer )
                {
                    _isShowingInformer = false;
                    _targetInformer.Hide();
                }
                
                return;
            }
            
            if ( _contextHandlerComposite != null )
            {
                _contextHandlerComposite.Initialize( _currentObservable );
                _contextHandlerComposite.SetOptions( GetOptions( _contextHandlerComposite.GetContextMenuOptions() ) );
                _contextHandlerComposite.OnCompleted += ContextCompleted;
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
        
        
        private void ContextCompleted()
        {
            // OnCompleted?.Invoke();
            // CurrentObservableChangedHandler( _currentObservable );
        }
    }
}