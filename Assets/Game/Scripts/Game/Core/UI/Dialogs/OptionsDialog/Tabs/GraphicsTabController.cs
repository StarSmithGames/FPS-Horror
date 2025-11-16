using Cysharp.Threading.Tasks;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class GraphicsTabController : TabController
    {
        private OptionSelectorController _resolutionOption;
        private OptionToggleController _fullScreenOption;
        private OptionToggleController _vsyncOption;

        private readonly GraphicsData _data;
        
        public GraphicsTabController(
            TabsSettings settings,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
            ) : base( settings, localizationSystem )
        {
            _data = dataHolder.GeneralStorageData.Graphics.Value;
        }

        public override void Save()
        {
            base.Save();
            
            if ( _isInitialized )
            {
                var resolution = Screen.resolutions[ _resolutionOption.Index ];
                _data.ResolutionWidth = resolution.width;
                _data.ResolutionHeight = resolution.height;
                _data.IsFullScreen = _fullScreenOption.IsOn;
                _data.IsVsync = _vsyncOption.IsOn;
                
                Screen.SetResolution( _data.ResolutionWidth, _data.ResolutionHeight, _data.IsFullScreen );
                QualitySettings.vSyncCount = _data.IsVsync ? 1 : 0;
            }
            Application.targetFrameRate = 60;
        }

        protected override async UniTask Load( Transform content, CancellationToken cancellationToken = default )
        {
            var resolutions = Screen.resolutions.ToList();
            _resolutionOption = CreateSelector( content, LocalizationIds.UI_OPTIONS_DIALOG_RESOLUTION, resolutions.IndexOf( Screen.currentResolution ), resolutions.Select( ( x ) => $"{x.width}x{x.height}" ).ToArray() );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
                
            _fullScreenOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_FULL_SCREEN, _data.IsFullScreen );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();

            _vsyncOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_VSYNC, _data.IsVsync );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
        }
    }
}