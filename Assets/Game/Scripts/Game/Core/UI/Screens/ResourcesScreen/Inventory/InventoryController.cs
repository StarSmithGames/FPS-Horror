using Cysharp.Threading.Tasks;
using Game.Core.Player;
using Game.Core.UI.ContextMenu;
using Game.Core.World.InventorySystem;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class InventoryController
    {
        private PlayerInventoryController _inventoryController;
        private PlayerEquipmentController _equipmentController;
        
        private List< UIInventoryCell > _inventoryCells = new();
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly UIInventory _view;
        private readonly ContextMenuController _contextMenuController;
        private readonly PlayerControllersService _playerControllersService;
        private readonly ILocalizationSystem _localizationSystem;
        
        public InventoryController(
            UIInventory view,
            ContextMenuController contextMenuController,
            PlayerControllersService playerControllersService,
            ILocalizationSystem localizationSystem
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _contextMenuController = contextMenuController ?? throw new ArgumentNullException( nameof(contextMenuController) );
            _playerControllersService = playerControllersService ?? throw new ArgumentNullException( nameof(playerControllersService) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize()
        {
            _inventoryController = _playerControllersService.GetAs< PlayerInventoryController >();
            _inventoryController.Inventory.OnChanged += PlayerInventoryChangedHandler;
            
            _equipmentController = _playerControllersService.GetAs< PlayerEquipmentController >();
            _equipmentController.OnEquipChanged += PlayerEquipChangedHandler;
        }

        public void Dispose()
        {
            _inventoryController.Inventory.OnChanged -= PlayerInventoryChangedHandler;
            _equipmentController.OnEquipChanged -= PlayerEquipChangedHandler;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void LoadInventory()
        {
            _cancellationTokenSource = new();
            LoadInventoryItems( _cancellationTokenSource.Token ).Forget();
        }
        
        private async UniTask LoadInventoryItems( CancellationToken cancellationToken = default )
        {
            _view.Content.DestroyChildren();
            _inventoryCells.Clear();

            for ( int i = 0; i < 20; i++ )
            {
                // var cell = _diContainer.InstantiatePrefab( ModelView.Inventory.CellPrefab, ModelView.Inventory.Content ).GetComponent< UIInventoryCell >();
                var cell = GameObject.Instantiate( _view.CellPrefab, _view.Content );
                cell.OnPointerEntered += PointerEnteredHandler;
                cell.OnPointerExited += PointerExitedHandler;
                cell.OnPointerClicked += PointerClickedHandler;
                
                var inventory = _inventoryController.Inventory;
                if ( i < inventory.Items.Count )
                {
                    cell.Set( inventory.Items[ i ] );
                    cell.SetEquip( _equipmentController.IsEquipped( cell.InventoryItem ) );
                    
                    cell.SetLock( false );
                }
                else
                {
                    cell.SetLock( true );
                }
                
                _inventoryCells.Add( cell );
            }
        }
        
        private void PointerEnteredHandler( UIInventoryCell cell )
        {
            if ( cell.InventoryItem == null ) return;

            _view.Description.Set(
                cell.InventoryItem.Config.NameId,//_localizationSystem.Translate( cell.Item.Config.NameId ),
                cell.InventoryItem.Config.DescriptionId,//_localizationSystem.Translate( cell.Item.Config.DescriptionId ),
                "Common"
                );
            _view.Description.Enable( true );
        }
        
        private void PointerExitedHandler( UIInventoryCell cell )
        {
            _view.Description.Enable( false );
        }

        private void PointerClickedHandler( UIInventoryCell cell )
        {
            if ( cell.InventoryItem == null )
            {
                _contextMenuController.HideContextMenu();
                return;
            }

            _contextMenuController.ShowContextMenu( cell.InventoryItem, (RectTransform)cell.transform );
        }

        private void PlayerInventoryChangedHandler()
        {
            var inventory = _inventoryController.Inventory;
            for ( int i = 0; i < _inventoryCells.Count; i++ )
            {
                var cell = _inventoryCells[ i ];
                if ( i < inventory.Items.Count )
                {
                    cell.Set( inventory.Items[ i ] );
                    cell.SetEquip( _equipmentController.IsEquipped( cell.InventoryItem ) );
                    
                    cell.SetLock( false );
                }
                else
                {
                    cell.SetLock( true );
                }
            }
        }
        
        private void PlayerEquipChangedHandler()
        {
            for ( int i = 0; i < _inventoryCells.Count; i++ )
            {
                var cell = _inventoryCells[ i ];
                if ( cell.IsEmpty ) continue;

                cell.SetEquip( _equipmentController.IsEquipped( cell.InventoryItem ) );
            }
        }
    }
}