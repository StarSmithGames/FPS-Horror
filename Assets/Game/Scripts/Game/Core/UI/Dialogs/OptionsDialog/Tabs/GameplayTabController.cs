using Cysharp.Threading.Tasks;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class GameplayTabController : TabController
    {
        private OptionToggleController _sprintOption;
        private OptionToggleController _crouchOption;
        private OptionBarController _mouseSensitivityOption;
        private OptionBarController _controllerXSensitivityOption;
        private OptionBarController _controllerYSensitivityOption;
        private OptionToggleController _controllerXInvertOption;
        private OptionToggleController _controllerYInvertOption;

        private readonly GameplayData _data;
        
        public GameplayTabController(
            TabsSettings settings,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
            ) : base( settings, localizationSystem )
        {
            _data = dataHolder.GeneralStorageData.Gameplay.Value;
        }

        public override void Save()
        {
            base.Save();
            
            if ( _isInitialized )
            {
                _data.IsSprintToggle = _sprintOption.IsOn;
                _data.IsCrouchToggle = _crouchOption.IsOn;
                _data.MouseXSensitivity = _mouseSensitivityOption.Value;
                _data.MouseYSensitivity = _mouseSensitivityOption.Value;
                _data.ControllerXSensitivity = _controllerXSensitivityOption.Value;
                _data.ControllerYSensitivity = _controllerYSensitivityOption.Value;
                _data.IsControllerInvertXToggle = _controllerXInvertOption.IsOn;
                _data.IsControllerInvertYToggle = _controllerYInvertOption.IsOn;
            }
        }
        
        protected override async UniTask Load( Transform content, CancellationToken cancellationToken = default )
        {
            _sprintOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_TOGGLE_SPRINT, _data.IsSprintToggle );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _crouchOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_TOGGLE_CROUCH, _data.IsCrouchToggle );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _mouseSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MOUSE_SENSITIVITY, _data.MouseXSensitivity, 2, 10, postfix: "" );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _controllerXSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_X_SENSITIVITY, _data.ControllerXSensitivity, 35, 150, postfix: "" );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _controllerYSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_Y_SENSITIVITY, _data.ControllerYSensitivity, 35, 150, postfix: "" );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _controllerXInvertOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_X_INVERT, _data.IsControllerInvertXToggle );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
            
            _controllerYInvertOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_Y_INVERT, _data.IsControllerInvertYToggle );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
        }
    }
}