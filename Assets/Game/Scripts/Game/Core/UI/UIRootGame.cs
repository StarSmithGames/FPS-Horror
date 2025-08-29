using Game.UISystem;
using PuzzlescapeGames.VVM;
using UnityEngine;
using Zenject;

namespace Game.Core.UI
{
    public sealed class UIRootGame : MonoBehaviour
    {
        [ field: SerializeField ] public UIDynamicScreen DynamicScreen { get; private set; }

        [ SerializeField ] private Transform _screensRoot;
        
        public ViewModelAggregator ScreenAggregator { get; private set; }
        public ViewModelAggregator DialogAggregator { get; private set; }
        
        [ Inject ]
        private void Construct(
            DiContainer diContainer,
            UISettings uiSettings
            )
        {
            ScreenAggregator = ViewModelAggregator.Create( diContainer, _screensRoot, uiSettings.Screens );
            DialogAggregator = ViewModelAggregator.Create( diContainer, DynamicScreen.DialogsRoot, uiSettings.Dialogs );
        }
        
        private void OnDestroy()
        {
            DialogAggregator?.Dispose();
            ScreenAggregator?.Dispose();
        }
    }
}