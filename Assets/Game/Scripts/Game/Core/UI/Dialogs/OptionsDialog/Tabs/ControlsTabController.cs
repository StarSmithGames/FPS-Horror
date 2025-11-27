using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = Game.Managers.InputManager.DeviceType;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class ControlsTabController : TabController
    {
        private List< OptionKeyController > _keys = new();

        private readonly InputKeyActionsSettings _keyActionsSettings;
        private readonly ControlsData _data;
        
        public ControlsTabController(
            TabsSettings settings,
            InputKeyActionsSettings keyActionsSettings,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
            ) : base( settings, localizationSystem )
        {
            _keyActionsSettings = keyActionsSettings ?? throw new ArgumentNullException( nameof(keyActionsSettings) );
            _data = dataHolder.GeneralStorageData.Controls.Value;
        }

        public override void Save()
        {
            base.Save();
        }

        protected override async UniTask Load( Transform content, CancellationToken cancellationToken = default )
        {
            //LocalizationIds.ITEM_DOOR
            // , InputManager.Inputs.Player.Movement.

            var movement = InputManager.Inputs.Player.Movement;
            
            #region Movement
            var movementKeyboard = RebindKeyService.GetBindingsForDevice( movement, DeviceType.Keyboard );

            var forwardKey = CreateKey( content, "Forward" );
            forwardKey.ViewKey.Keyboard.SetType( false );
            forwardKey.ViewKey.Mouse.SetType( false );
            forwardKey.ViewKey.Gamepad.SetType( true );
            forwardKey.ViewKey.Keyboard.SetKey( movementKeyboard[ 0 ].Key );
            forwardKey.ViewKey.Mouse.Block( true );
            forwardKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadMovementTop );

            var backwardKey = CreateKey( content, "Backward" );
            backwardKey.ViewKey.Keyboard.SetType( false );
            backwardKey.ViewKey.Mouse.SetType( false );
            backwardKey.ViewKey.Gamepad.SetType( true );
            backwardKey.ViewKey.Keyboard.SetKey( movementKeyboard[ 1 ].Key );
            backwardKey.ViewKey.Mouse.Block( true );
            backwardKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadMovementBottom );
            
            var leftKey = CreateKey( content, "Left" );
            leftKey.ViewKey.Keyboard.SetType( false );
            leftKey.ViewKey.Mouse.SetType( false );
            leftKey.ViewKey.Gamepad.SetType( true );
            leftKey.ViewKey.Keyboard.SetKey( movementKeyboard[ 2 ].Key );
            leftKey.ViewKey.Mouse.Block( true );
            leftKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadMovementLeft );
            
            var rightKey = CreateKey( content, "Right" );
            rightKey.ViewKey.Keyboard.SetType( false );
            rightKey.ViewKey.Mouse.SetType( false );
            rightKey.ViewKey.Gamepad.SetType( true );
            rightKey.ViewKey.Keyboard.SetKey( movementKeyboard[ 3 ].Key );
            rightKey.ViewKey.Mouse.Block( true );
            rightKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadMovementRight );
            #endregion

            var sprintKey = CreateKey( content, "Sprint" );
            sprintKey.ViewKey.Keyboard.SetType( false );
            sprintKey.ViewKey.Mouse.SetType( false );
            sprintKey.ViewKey.Gamepad.SetType( true );
            sprintKey.ViewKey.Keyboard.SetKey( "L Shift" );
            sprintKey.ViewKey.Mouse.Block( true );
            sprintKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadSprint );
            
            var crouchKey = CreateKey( content, "Crouch" );
            crouchKey.ViewKey.Keyboard.SetType( false );
            crouchKey.ViewKey.Mouse.SetType( false );
            crouchKey.ViewKey.Gamepad.SetType( true );
            crouchKey.ViewKey.Keyboard.SetKey( "L Ctrl" );
            crouchKey.ViewKey.Mouse.Block( true );
            crouchKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadCrouch );
            
            var interactKey = CreateKey( content, "Interact" );
            interactKey.ViewKey.Keyboard.SetType( false );
            interactKey.ViewKey.Mouse.SetType( false );
            interactKey.ViewKey.Gamepad.SetType( true );
            interactKey.ViewKey.Keyboard.SetKey( RebindKeyService.GetBindingsForDevice( InputManager.Inputs.Player.Interact, DeviceType.Keyboard ).First().Key );
            interactKey.ViewKey.Mouse.Block( true );
            interactKey.ViewKey.Gamepad.SetIcon( _keyActionsSettings.GamepadInteract );
            
            _keys.Add( forwardKey );
            _keys.Add( backwardKey );
            _keys.Add( leftKey );
            _keys.Add( rightKey );
            _keys.Add( sprintKey );
            _keys.Add( crouchKey );
            _keys.Add( interactKey );
            // _keys.Add( CreateKey( content, "Reload weapon", "R" ) );

            for ( int i = 0; i < _keys.Count; i++ )
            {
                _keys[ i ].OnKeyButtonClicked += ButtonClickedHandler;
            }
        }
        
        public void PrintMovementBindings(InputAction movement)
        {
            movement.actionMap?.Enable(); // важно для effectivePath

            var bindings = movement.bindings;

            Debug.Log("=== Movement Bindings ===");

            foreach (var b in bindings)
            {
                // Игнорируем композиты (они — контейнеры)
                if (b.isComposite)
                {
                    Debug.Log($"Composite: {b.name}");
                    continue;
                }

                // Путь к кнопке/стеку устройства
                string path = b.effectivePath; // Лучше, чем b.path
                if (string.IsNullOrEmpty(path))
                    path = b.path;

                // Имя устройства
                string device = InputControlPath.TryGetDeviceLayout(path);

                // Красивое имя кнопки
                string humanReadable = InputControlPath.ToHumanReadableString(
                    path,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );

                Debug.Log($"{b.name} | Device={device} | Path={path} | Key={humanReadable}");
            }
        }

        private string GetKeyName( string path )
        {
            return InputControlPath.ToHumanReadableString( path, InputControlPath.HumanReadableStringOptions.OmitDevice );
        }

        protected override void ButtonClickedHandler( OptionController uiOption )
        {
            Debug.LogError( "Set" );
            
            //var action = InputManager.Inputs.FindAction( actionName );

            // string json = InputManager.Inputs.SaveBindingOverridesAsJson();
            // InputManager.Inputs.LoadBindingOverridesFromJson( json );
            // InputManager.Inputs.RemoveAllBindingOverrides();
            // Debug.LogError( json );

        }
    }
}