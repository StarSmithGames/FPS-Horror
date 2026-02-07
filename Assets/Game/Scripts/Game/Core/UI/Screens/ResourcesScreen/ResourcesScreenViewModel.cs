using Cysharp.Threading.Tasks;
using Game.Core.World.WorldManager;
using Game.Core.Player;
using Game.Core.UI.ContextMenu;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using Game.Core.World.InventorySystem;
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
        private int _currentTabIndex = -1;
        private InputActionVoidWrap _inputActionCancel;
        private InputActionVoidWrap _inputActionInventory;
        private CancellationTokenSource _cancellationInventorySource;
        private List< UIInventoryCell > _inventoryCells = new();
        private CancellationTokenSource _cancellationLibrarySource;
        private List< UILibraryOption > _libraryOptions = new();
        private ContextMenuController _contextMenuController;
            
        private readonly DiContainer _diContainer;
        private readonly ItemDescriptor _itemDescriptor;
        private readonly PauseManager _pauseManager;
        private readonly GameManager _gameManager;
        private readonly PlayerControllersService _playerControllersService;
        
        public ResourcesScreenViewModel(
            DiContainer diContainer,
            ItemDescriptor itemDescriptor,
            PauseManager pauseManager,
            GameManager gameManager,
            PlayerControllersService playerControllersService
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
            _itemDescriptor = itemDescriptor ?? throw new ArgumentNullException( nameof(itemDescriptor) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _playerControllersService = playerControllersService ?? throw new ArgumentNullException( nameof(playerControllersService) );
        }
        
        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
            
            _inputActionInventory = new( InputManager.Inputs.System.Inventory, InventoryClickedHandler );
            _inputActionInventory.Enable();

            for ( int i = 0; i < ModelView.MenuOptions.Count; i++ )
            {
                ModelView.MenuOptions[ i ].OnButtonClicked += MenuOptionClickedHandler;
            }
            ModelView.OnBackButtonClicked += BackButtonClickedHandler;

            _playerControllersService.GetAs< PlayerEquipmentController >().OnEquipChanged += PlayerEquipChangedHandler;
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            _playerControllersService.GetAs< PlayerEquipmentController >().OnEquipChanged -= PlayerEquipChangedHandler;
            
            for ( int i = 0; i < ModelView.MenuOptions.Count; i++ )
            {
                ModelView.MenuOptions[ i ].OnButtonClicked -= MenuOptionClickedHandler;
            }
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
                _cancellationInventorySource?.Cancel();
                _cancellationInventorySource?.Dispose();
                _cancellationInventorySource = null;
                
                _cancellationLibrarySource?.Cancel();
                _cancellationLibrarySource?.Dispose();
                _cancellationLibrarySource = null;
                
                CursorManager.Disable();
                _pauseManager.UnPause();
                _gameManager.SetState( GameState.Game );
                return;
            }
            CursorManager.Enable();
            _pauseManager.Pause();
            _gameManager.SetState( GameState.Menu );
            
            _contextMenuController = _diContainer.Instantiate< ContextMenuController >( new object[] { ModelView.ContextMenu } );
            _contextMenuController.Initialize();
            
            // EventSystem.current.SetSelectedGameObject( ModelView.ContinueButton.gameObject );
            SelectTab( 1 );
        }

        private void SelectTab( int index )
        {
            _currentTabIndex = index;

            for ( int i = 0; i < ModelView.MenuOptions.Count; i++ )
            {
                ModelView.MenuOptions[ i ].Deselect();
            }
            ModelView.MenuOptions[ index ].Select();

            if ( index == 0 )
            {
                
            }
            else if ( index == 1 )
            {
                ModelView.Inventory.gameObject.SetActive( true );
                ModelView.Inventory.Description.Enable( false );
                ModelView.Library.gameObject.SetActive( false );

                _cancellationInventorySource = new();
                LoadInventoryItems( _cancellationInventorySource.Token ).Forget();
            }
            else if ( index == 2 )
            {
                ModelView.Inventory.gameObject.SetActive( false );
                ModelView.Library.gameObject.SetActive( true );

                _cancellationLibrarySource = new();
                LoadLibraryItems( _cancellationLibrarySource.Token ).Forget();
            }
        }
        
        private async UniTask LoadInventoryItems( CancellationToken cancellationToken = default )
        {
            ModelView.Inventory.Content.DestroyChildren();
            _inventoryCells.Clear();

            var inventory = _playerControllersService.GetAs< PlayerInventoryController >().Inventory;
            var equipment = _playerControllersService.GetAs< PlayerEquipmentController >();
            
            for ( int i = 0; i < 20; i++ )
            {
                var cell = _diContainer.InstantiatePrefab( ModelView.Inventory.CellPrefab, ModelView.Inventory.Content ).GetComponent< UIInventoryCell >();
                cell.OnPointerEntered += PointerEnteredHandler;
                cell.OnPointerExited += PointerExitedHandler;
                cell.OnPointerClicked += PointerClickedHandler;
                
                if ( i < inventory.Items.Count )
                {
                    cell.Set( inventory.Items[ i ] );
                    cell.SetEquip( equipment.IsEquipped( cell.Item ) );
                    
                    cell.SetLock( false );
                }
                else
                {
                    cell.SetLock( true );
                }
                
                _inventoryCells.Add( cell );
            }
        }

        private async UniTask LoadLibraryItems( CancellationToken cancellationToken = default )
        {
            var library = _playerControllersService.GetAs< PlayerLibraryController >();
            
            ModelView.Library.Content.DestroyChildren();
            ModelView.Library.MainText.text = string.Empty;
            _libraryOptions.Clear();

            for ( int i = 0; i < library.Library.Items.Count; i++ )
            {
                var item = GameObject.Instantiate( ModelView.Library.OptionPrefab, ModelView.Library.Content );
                item.SetText( library.GetItemName( library.Library.Items[ i ] ) );
                
                _libraryOptions.Add( item );
            }
        }
        
        private void MenuOptionClickedHandler( UIOption uiOption )
        {
            var index = ModelView.MenuOptions.IndexOf( (UIOptionMenuButton)uiOption );
            SelectTab( index );
        }
        
        private void PlayerEquipChangedHandler()
        {
            var controller = _playerControllersService.GetAs< PlayerEquipmentController >();
            
            for ( int i = 0; i < _inventoryCells.Count; i++ )
            {
                var cell = _inventoryCells[ i ];
                if ( cell.IsEmpty ) continue;

                cell.SetEquip( controller.IsEquipped( cell.Item ) );
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