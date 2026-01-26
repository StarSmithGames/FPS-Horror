using Game.Systems.InventorySystem;
using Game.Systems.InventorySystem.ContextMenu;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.ContextMenu
{
    public sealed class ContextMenuController
    {
        private readonly UIContextMenu _view;
        private readonly ContextMenuService _contextMenuService;
        private readonly ILocalizationSystem _localizationSystem;
        
        public ContextMenuController(
            UIContextMenu view,
            ContextMenuService contextMenuService,
            ILocalizationSystem localizationSystem
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _contextMenuService = contextMenuService ?? throw new ArgumentNullException( nameof(contextMenuService) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Initialize()
        {
            _view.Enable( false );
        }

        public void ShowContextMenu( ItemModel model, RectTransform from )
        {
            _view.Content.DestroyChildren();
            
            var position = (Vector2)from.position;
            position += from.sizeDelta / 2;
            _view.Root.position = position;

            List< ContextMenuItem > menu = null;
            if ( model.Config is WeaponConfig )
            {
                menu = _contextMenuService.GetWeaponMenu( false );
            }
            else
            {
                menu = _contextMenuService.GetItemMenu( true );
            }
            
            for ( int i = 0; i < menu.Count; i++ )
            {
                var context = menu[ i ];
                var item = GameObject.Instantiate( _view.ItemPrefab, _view.Content );
                item.Set( context, _localizationSystem.Translate( context.NameId ) );
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
            }
        }
    }
}