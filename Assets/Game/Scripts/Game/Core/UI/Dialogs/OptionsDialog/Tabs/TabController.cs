using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI.OptionsDialog
{
    public abstract class TabController
    {
        public List< OptionController > Options { get; private set; } = new();
        private OptionController _lastOption;
        
        protected bool _isInitialized;
        private InputActionValueWrap< Vector2 > _inputActionNavigate;
        
        protected readonly TabsSettings _settings;
        private readonly ILocalizationSystem _localizationSystem;

        public TabController(
            TabsSettings settings,
            ILocalizationSystem localizationSystem
            )
        {
            _settings = settings ?? throw new ArgumentNullException( nameof(settings) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public void Subscribe()
        {
            _inputActionNavigate = InputActionManager.CreateInputActionWrap< Vector2 >( InputManager.Inputs.UI.Navigate, NavigateChangedHandler );
            _inputActionNavigate.Enable();
        }

        public void Dispose()
        {
            InputActionManager.RemoveInputActionWrap( _inputActionNavigate );
            
            Options.Clear();
        }

        public async UniTask InitializeAndLoad( Transform content, CancellationToken cancellationToken = default )
        {
            if ( !_isInitialized )
            {
                _isInitialized = true;
                Load( content, cancellationToken ).Forget();
                await UniTask.Yield();
            }
            EventSystem.current.SetSelectedGameObject( Options.First().View.gameObject );
        }

        public virtual void Save()
        {
            for ( int i = 0; i < Options.Count; i++ )
            {
                Options[ i ].ResetDirty();
            }
        }

        public bool IsDirty() => Options.Any( ( x ) => x.IsDirty );
        
        protected abstract UniTask Load( Transform content, CancellationToken cancellationToken = default );
        
        protected OptionSelectorController CreateSelector( Transform content, string nameId, int value, params string[] options )
        {
            OptionSelectorController option = new( GameObject.Instantiate( _settings.OptionLeftRightPrefab, content ), value );
            option.SetName( _localizationSystem.Translate( nameId ) );
            option.Initialize( options );
            option.OnButtonClicked += ButtonClickedHandler;
            option.OnPointerEntered += PointerEnteredHandler;
            option.OnPointerExited += PointerExitedHandler;
                
            Options.Add( option );

            return option;
        }
            
        protected OptionBarController CreateBar( Transform content, string nameId, float value, float min = 0, float max = 100, float step = 1, string postfix = "%" )
        {
            OptionBarController option = new( GameObject.Instantiate( _settings.OptionLeftRightPrefab, content ), value, min, max, step, postfix );
            option.SetName( _localizationSystem.Translate( nameId ) );
            option.Initialize();
            option.OnButtonClicked += ButtonClickedHandler;
            option.OnPointerEntered += PointerEnteredHandler;
            option.OnPointerExited += PointerExitedHandler;
                
            Options.Add( option );

            return option;
        }

        protected OptionToggleController CreateToggle( Transform content, string nameId, bool value )
        {
            OptionToggleController option = new( GameObject.Instantiate( _settings.OptionTogglePrefab, content ), value );
            option.SetName( _localizationSystem.Translate( nameId ) );
            option.Initialize();
            option.OnButtonClicked += ButtonClickedHandler;
            option.OnPointerEntered += PointerEnteredHandler;
            option.OnPointerExited += PointerExitedHandler;

            Options.Add( option );

            return option;
        }

        protected OptionKeyController CreateKey( Transform content, string nameId )
        {
            OptionKeyController option = new( GameObject.Instantiate( _settings.OptionKeyPrefab, content ) );
            option.SetName( nameId ); //_localizationSystem.Translate( nameId ) );
            option.Initialize();
            option.OnButtonClicked += ButtonClickedHandler;
            option.OnPointerEntered += PointerEnteredHandler;
            option.OnPointerExited += PointerExitedHandler;

            Options.Add( option );

            return option;
        }
        
        protected virtual void PointerEnteredHandler( OptionController option )
        {
            for ( int i = 0; i < Options.Count; i++ )
            {
                Options[ i ].Deselect();
            }
            option.Select();

            _lastOption = option;
        }

        protected virtual void PointerExitedHandler( OptionController uiOption ) {}
        
        protected virtual void ButtonClickedHandler( OptionController uiOption ) {}

        private void NavigateChangedHandler( Vector2 value )
        {
            if ( _lastOption is OptionBarController percentOption )
            {
                if ( value.x < 0 )
                {
                    percentOption.Left();
                }
                else if ( value.x > 0 )
                {
                    percentOption.Right();
                }
            }
            else if ( _lastOption is OptionSelectorController selectorOption )
            {
                if ( value.x < 0 )
                {
                    selectorOption.Left();
                }
                else if ( value.x > 0 )
                {
                    selectorOption.Right();
                }
            }
        }
    }
}