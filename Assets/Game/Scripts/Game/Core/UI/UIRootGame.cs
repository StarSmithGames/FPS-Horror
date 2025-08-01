using PuzzlescapeGames.VVM;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Core.UI
{
    public sealed class UIRootGame : MonoBehaviour
    {
        [ field: SerializeField ] public UIMenuScreen MenuScreen { get; private set; }
        [ field: SerializeField ] public UIGameScreen GameScreen { get; private set; }
        [ field: SerializeField ] public UIDynamicScreen DynamicScreen { get; private set; }
        
        public ViewModelAggregator ScreenAggregator { get; private set; }
        public ViewModelAggregator DialogAggregator { get; private set; }
        
        private List< Type > _runtimeViewModels = new()
        {
            typeof(MenuScreenViewModel),
            typeof(GameScreenViewModel),
        };
        
        [ Inject ]
        private void Construct(
            DiContainer diContainer,
            UISettings uiSettings
            )
        {
            ScreenAggregator = ViewModelAggregator.Create( diContainer, transform, Array.Empty< View >() );
            DialogAggregator = ViewModelAggregator.Create( diContainer, DynamicScreen.DialogsRoot, uiSettings.Dialogs );
        }
        
        public void Initialize()
        {
            // var camera = Camera.main;
            // for ( int i = 0; i < _canvases.Count; i++ )
            // {
            //     _canvases[ i ].worldCamera = camera;
            // }
			
            for ( int i = 0; i < _runtimeViewModels.Count; i++ )
            {
                ScreenAggregator.Create( _runtimeViewModels[ i ] );
            }
        }
        
        private void OnDestroy()
        {
            DialogAggregator?.Dispose();
            ScreenAggregator?.Dispose();
        }
    }
}