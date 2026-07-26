using Game.Core.Player;
using Game.Core.UI.ContextMenu;
using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using System;
using Zenject;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class ResourcesScreenViewModel : ViewModel< UIResourcesScreen >
    {
        private int _currentTabIndex = -1;
        private InputActionVoidWrap _inputActionCancel;
        private InputActionVoidWrap _inputActionInventory;
        private InventoryController _inventoryController;
        private LibraryController _libraryController;
        private ContextMenuController _contextMenuController;
            
        private readonly DiContainer _diContainer;
        private readonly PlayerControllersService _playerControllersService;
        
        public ResourcesScreenViewModel(
            DiContainer diContainer,
            PlayerControllersService playerControllersService
            )
        {
            _diContainer = diContainer ?? throw new ArgumentNullException( nameof(diContainer) );
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
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
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
                _inventoryController?.Dispose();
                _inventoryController = null;
                
                _libraryController?.Dispose();
                _libraryController = null;
                return;
            }
            _contextMenuController = _diContainer.Instantiate< ContextMenuController >( new object[] { ModelView.ContextMenu } );
            _contextMenuController.Initialize();

            _playerControllersService.GetAs< PlayerJournalController >().GetCurrentObjective();
            
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

                if ( _inventoryController == null )
                {
                    _inventoryController = _diContainer.Instantiate< InventoryController >( new object[] { ModelView.Inventory, _contextMenuController } );
                    _inventoryController.Initialize();
                }
                _inventoryController.LoadInventory();
            }
            else if ( index == 2 )
            {
                ModelView.Inventory.gameObject.SetActive( false );
                ModelView.Library.gameObject.SetActive( true );

                if ( _libraryController == null )
                {
                    _libraryController = _diContainer.Instantiate< LibraryController >( new object[] { ModelView.Library } );
                }
                _libraryController.LoadLibrary();
            }
        }
        
        private void MenuOptionClickedHandler( UIOption uiOption )
        {
            var index = ModelView.MenuOptions.IndexOf( (UIOptionMenuButton)uiOption );
            SelectTab( index );
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