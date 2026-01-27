using Game.Core.Player;
using Game.Core.World.InventorySystem;
using Game.Core.World.InventorySystem.ContextMenu;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.ContextMenu
{
    public sealed class ContextMenuController
    {
        private List< UIContextMenuItem > _items = new();
        
        private readonly UIContextMenu _view;
        private readonly ContextMenuService _contextMenuService;
        private readonly PlayerControllersService _playerControllersService;
        private readonly ILocalizationSystem _localizationSystem;
        
        public ContextMenuController(
            UIContextMenu view,
            ContextMenuService contextMenuService,
            PlayerControllersService playerControllersService,
            ILocalizationSystem localizationSystem
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _contextMenuService = contextMenuService ?? throw new ArgumentNullException( nameof(contextMenuService) );
            _playerControllersService = playerControllersService ?? throw new ArgumentNullException( nameof(playerControllersService) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        private void Clear()
        {
            for ( int i = 0; i < _items.Count; i++ )
            {
                var item = _items[ i ];
                item.OnPointerEntered -= PointerEnteredHandler;
                item.OnPointerExited -= PointerExitedHandler;
                item.OnPointerClicked -= PointerClickedHandler;
            }
            _items.Clear();
        }
        
        public void Initialize()
        {
            _view.Enable( false );
        }

        public void ShowContextMenu( ItemModel model, RectTransform from )
        {
            Clear();
            _view.Content.DestroyChildren();
            
            var position = (Vector2)from.position;
            position += from.sizeDelta / 2;
            _view.Root.position = position;

            List< MenuItemCommand > menu = null;
            if ( model.Config is WeaponConfig )
            {
                menu = _contextMenuService.GetWeaponMenu( model, _playerControllersService.GetAs< PlayerEquipmentController >().IsEquipped( model ) );
            }
            else
            {
                menu = _contextMenuService.GetItemMenu( model );
            }
            
            for ( int i = 0; i < menu.Count; i++ )
            {
                var itemCommand = menu[ i ];
                var item = GameObject.Instantiate( _view.ItemPrefab, _view.Content );
                item.Set( itemCommand, _localizationSystem.Translate( itemCommand.Item.NameId ) );
                item.OnPointerEntered += PointerEnteredHandler;
                item.OnPointerExited += PointerExitedHandler;
                item.OnPointerClicked += PointerClickedHandler;
                
                _items.Add( item );
            }
            
            if ( !_view.IsShowing )
            {
                _view.Show();
            }
        }

        public void HideContextMenu()
        {
            if ( _view.IsShowing )
            {
                _view.Hide();
                Clear();
            }
        }

        private void PointerEnteredHandler( UIContextMenuItem item )
        {
            for ( int i = 0; i < _items.Count; i++ )
            {
                _items[ i ].Deselect();
            }
            item.Select();
        }
        
        private void PointerExitedHandler( UIContextMenuItem item )
        {
            
        }
        
        private void PointerClickedHandler( UIContextMenuItem item )
        {
            if ( !item.ItemCommand.Command.CanExecute() ) return;
            item.ItemCommand.Command.Execute();
            HideContextMenu();
        }
    }
}