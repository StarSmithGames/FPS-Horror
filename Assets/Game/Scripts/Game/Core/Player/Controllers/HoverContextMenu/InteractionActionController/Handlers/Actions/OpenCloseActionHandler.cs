using Game.Core.Entity;
using Game.Core.UI;
using Game.Managers.InputManager;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class OpenCloseActionHandler : LongActionHandler
    {
        private OpenCloseObject _dynamic;

        public OpenCloseActionHandler(
            UIRootGame uiRootGame,
            InputKeyActionsSettings inputKeyActionsSettings
            ) : base( uiRootGame, inputKeyActionsSettings.InteractAction )
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

        public override void Enable()
        {
            base.Enable();
            
            _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( false );
            _gameScreenViewModel.ModelView.TargetHand.SetHand( true );
            _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( true );
        }

        protected override void InteractStarted()
        {
            base.InteractStarted();
            _gameScreenViewModel.ModelView.TargetHand.SetHand( false );
        }

        protected override void InteractFinished( bool result )
        {
            base.InteractFinished( result );
            
            if ( !IsEnable ) return;
            
            _gameScreenViewModel.ModelView.TargetHand.SetHand( true );
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
         
            _gameScreenViewModel.ModelView.TargetHand.SetHand( true );
            
            base.Completed();
        }
    }
}