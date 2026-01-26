using Cysharp.Threading.Tasks;
using Game.Core.World.WorldManager;
using Game.Core.Player;
using Game.Core.UI.ContextMenu;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using Game.Systems.InventorySystem;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.VVM;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class ResourcesScreenViewModel : ViewModel< UIResourcesScreen >
    {
        private InputActionVoidWrap _inputActionCancel;
        private InputActionVoidWrap _inputActionInventory;
        private CancellationTokenSource _cancellationTokenSource;
        private List< UIInventoryCell > _cells = new();
        private ContextMenuController _contextMenuController;
            
        private readonly DiContainer _diContainer;
        private readonly ItemDescriptor _itemDescriptor;
        private readonly PauseManager _pauseManager;
        private readonly GameManager _gameManager;
        private readonly WorldManager _worldManager;
        
        public ResourcesScreenViewModel(
            DiContainer diContainer,
            ItemDescriptor itemDescriptor,
            PauseManager pauseManager,
            GameManager gameManager,
            WorldManager worldManager
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _itemDescriptor = itemDescriptor ?? throw new ArgumentNullException( nameof(itemDescriptor) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _worldManager = worldManager ?? throw new ArgumentNullException( nameof(worldManager) );
        }
        
        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
            
            _inputActionInventory = new( InputManager.Inputs.System.Inventory, InventoryClickedHandler );
            _inputActionInventory.Enable();

            ModelView.OnBackButtonClicked += BackButtonClickedHandler;
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.OnBackButtonClicked -= BackButtonClickedHandler;
            
            _inputActionCancel.Disable();
            _inputActionInventory.Disable();
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
            InputActionManager.RemoveInputActionWrap( _inputActionInventory );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing )
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                
                CursorManager.Disable();
                _pauseManager.UnPause();
                _gameManager.SetState( GameState.Game );
                return;
            }
            CursorManager.Enable();
            _pauseManager.Pause();
            _gameManager.SetState( GameState.Menu );

            ModelView.Inventory.Description.Enable( false );
            
            for ( int i = 0; i < ModelView.MenuOptions.Count; i++ )
            {
                ModelView.MenuOptions[ i ].Deselect();
            }
            ModelView.MenuOptions[ 1 ].Select();
            
            _contextMenuController = _diContainer.Instantiate< ContextMenuController >( new object[] { ModelView.ContextMenu } );
            _contextMenuController.Initialize();
            
            // EventSystem.current.SetSelectedGameObject( ModelView.ContinueButton.gameObject );

            _cancellationTokenSource = new();
            LoadItems( _cancellationTokenSource.Token ).Forget();
        }

        private async UniTask LoadItems( CancellationToken cancellationToken = default )
        {
            ModelView.Inventory.Content.DestroyChildren();
            _cells.Clear();

            var inventoryController = _worldManager.Player.Controller.ServiceLocator.GetAs< PlayerInventoryController >();
            var inventory = inventoryController.Inventory;
            
            for ( int i = 0; i < 20; i++ )
            {
                var cell = _diContainer.InstantiatePrefab( ModelView.Inventory.CellPrefab, ModelView.Inventory.Content ).GetComponent< UIInventoryCell >();
                cell.OnPointerEntered += PointerEnteredHandler;
                cell.OnPointerExited += PointerExitedHandler;
                cell.OnPointerClicked += PointerClickedHandler;
                
                if ( i < inventory.Items.Count )
                {
                    cell.Set( inventory.Items[ i ] );
                    cell.SetLock( false );
                }
                else
                {
                    cell.SetLock( true );
                }
                
                _cells.Add( cell );
            }
        }
        
        private void PointerEnteredHandler( UIInventoryCell cell )
        {
            if ( cell.Item != null )
            {
                ModelView.Inventory.Description.Set( cell.Item.Config, _itemDescriptor );
                ModelView.Inventory.Description.Enable( true );
            }
        }
        
        private void PointerExitedHandler( UIInventoryCell cell )
        {
            ModelView.Inventory.Description.Enable( false );
        }

        private void PointerClickedHandler( UIInventoryCell cell )
        {
            if ( cell.Item == null )
            {
                _contextMenuController.HideContextMenu();
                return;
            }

            _contextMenuController.ShowContextMenu( cell.Item, (RectTransform)cell.transform );
        }
        
        private void InventoryClickedHandler()
        {
            HideViewAndDispose();
        }

        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }

        private void BackButtonClickedHandler()
        {
            _contextMenuController.HideContextMenu();
        }
    }
}