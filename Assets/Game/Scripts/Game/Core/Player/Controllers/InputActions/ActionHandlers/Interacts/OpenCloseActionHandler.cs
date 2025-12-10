using Game.Core.Entity;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System;

namespace Game.Core.Player
{
    public sealed class OpenCloseActionHandler : LongActionHandler
    {
        private OpenCloseDynamicObject _dynamicObject;
        
        private readonly ILocalizationSystem _localizationSystem;
        
        public OpenCloseActionHandler(
            InputKeyActionsSettings inputKeyActionsSettings,
            ILocalizationSystem localizationSystem
            ) : base( inputKeyActionsSettings.InteractAction )
        {
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public override void Initialize( IObservable target )
        {
            _dynamicObject = (OpenCloseDynamicObject)target;
            
            ContextMenuOperation.Key = _inputKeyAction.GetDisplayKey();
            string nameId = _dynamicObject.IsOpen ? LocalizationIds.UI_CONTROL_CLOSE : LocalizationIds.UI_CONTROL_OPEN;
            ContextMenuOperation.Name = _localizationSystem.Translate( nameId );
        }

        public override void Dispose()
        {
            _dynamicObject = null;

            base.Dispose();
        }

        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            if ( _dynamicObject.IsOpen )
            {
                _dynamicObject.Close();
            }
            else
            {
                _dynamicObject.Open();
            }
            
            base.Completed();
        }
    }
}