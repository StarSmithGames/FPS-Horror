using Cysharp.Threading.Tasks;
using Game.Core.Player;
using PuzzlescapeGames.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class LibraryController
    {
        private UILibraryOption _selectedOption;
        private List< UILibraryOption > _libraryOptions = new();
        private CancellationTokenSource _cancellationTokenSource;

        private readonly UILibrary _view;
        private readonly PlayerControllersService _playerControllersService;
        
        public LibraryController(
            UILibrary view,
            PlayerControllersService playerControllersService
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _playerControllersService = playerControllersService ?? throw new ArgumentNullException( nameof(playerControllersService) );
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            for ( int i = 0; i < _libraryOptions.Count; i++ )
            {
                var item = _libraryOptions[ i ];
                item.OnPointerEntered -= PointerEnteredHandler;
                item.OnPointerEntered -= PointerExitedHandler;
            }
        }
        
        public void LoadLibrary()
        {
            _cancellationTokenSource = new();
            LoadLibraryItems( _cancellationTokenSource.Token ).Forget();
        }
        
        private async UniTask LoadLibraryItems( CancellationToken cancellationToken = default )
        {
            var library = _playerControllersService.GetAs< PlayerLibraryController >();
            
            _view.Content.DestroyChildren();
            _view.MainText.text = string.Empty;
            _libraryOptions.Clear();

            for ( int i = 0; i < library.Library.Items.Count; i++ )
            {
                var item = GameObject.Instantiate( _view.OptionPrefab, _view.Content );
                item.Set( library.Library.Items[ i ] );
                item.SetText( library.GetItemName( item.Item ) );
                
                item.OnPointerEntered += PointerEnteredHandler;
                item.OnPointerExited += PointerExitedHandler;
                item.OnButtonClicked += ButtonClickedHandler;
                    
                _libraryOptions.Add( item );
            }
        }

        private void RefreshSelection()
        {
            var library = _playerControllersService.GetAs< PlayerLibraryController >();
            _view.MainText.text = library.GetItemDescription( _selectedOption.Item );
        }

        private void PointerEnteredHandler( UIOption option )
        {
            for ( int i = 0; i < _libraryOptions.Count; i++ )
            {
                if ( _libraryOptions[ i ] == _selectedOption ) continue;
                _libraryOptions[ i ].Deselect();
            }
            option.Select();
        }

        private void PointerExitedHandler( UIOption option )
        {
            if ( option == _selectedOption ) return;
            option.Deselect();
        }

        private void ButtonClickedHandler( UIOption option )
        {
            _selectedOption = (UILibraryOption)option;
            _selectedOption.Select();
            for ( int i = 0; i < _libraryOptions.Count; i++ )
            {
                if ( _libraryOptions[ i ] == _selectedOption ) continue;
                _libraryOptions[ i ].Deselect();
            }

            RefreshSelection();
        }
    }
}