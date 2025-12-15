using Cysharp.Threading.Tasks;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.VVM;
using System;
using System.Collections.Generic;
using System.Threading;
using Zenject;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class ResourcesScreenViewModel : ViewModel< UIResourcesScreen >
    {
        private InputActionVoidWrap _inputActionCancel;
        private CancellationTokenSource _cancellationTokenSource;
        private List< UIInventoryCell > _cells = new();
            
        private readonly DiContainer _diContainer;
        private readonly PauseManager _pauseManager;
        private readonly GameManager _gameManager;
        
        public ResourcesScreenViewModel(
            DiContainer diContainer,
            PauseManager pauseManager,
            GameManager gameManager
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
        }
        
        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing )
            {
                CursorManager.Disable();
                _pauseManager.UnPause();
                _gameManager.SetState( GameState.Game );
                return;
            }
            CursorManager.Enable();
            _pauseManager.Pause();
            _gameManager.SetState( GameState.Menu );

            for ( int i = 0; i < ModelView.MenuOptions.Count; i++ )
            {
                ModelView.MenuOptions[ i ].Deselect();
            }
            ModelView.MenuOptions[ 1 ].Select();
            
            // EventSystem.current.SetSelectedGameObject( ModelView.ContinueButton.gameObject );

            _cancellationTokenSource = new();
            LoadItems( _cancellationTokenSource.Token ).Forget();
        }

        private async UniTask LoadItems( CancellationToken cancellationToken = default )
        {
            ModelView.Inventory.Content.DestroyChildren();
            _cells.Clear();
            
            for ( int i = 0; i < 20; i++ )
            {
                var cell = _diContainer.InstantiatePrefab( ModelView.Inventory.CellPrefab, ModelView.Inventory.Content ).GetComponent< UIInventoryCell >();
                cell.SetLock( i > 4 );
                _cells.Add( cell );
            }
        }

        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }
    }
}