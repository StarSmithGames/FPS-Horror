using Game.Core.Entity;
using Game.Managers.InputManager;

namespace Game.Core.Player
{
    public sealed class OpenCloseActionHandler : LongActionHandler
    {
        private OpenCloseObject _dynamic;

        public OpenCloseActionHandler(
            InputKeyActionsSettings inputKeyActionsSettings
            ) : base( inputKeyActionsSettings.InteractAction )
        {
        }

        public void Set( OpenCloseObject dynamic )
        {
            _dynamic = dynamic;
        }
        
        public override void Dispose()
        {
            _dynamic = null;

            base.Dispose();
        }

        protected override void Completed()
        {
            if ( !IsEnable ) return;
            
            if ( _dynamic.IsOpen )
            {
                _dynamic.Close();
            }
            else
            {
                _dynamic.Open();
            }
            
            base.Completed();
        }
    }
}