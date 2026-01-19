using System;

namespace Game.Core.UI.GameScreen
{
    public sealed class PointerController
    {
        private PointerType _pointerType = PointerType.None;
        private bool _isObservablesAround;

        private readonly GameScreenViewModel _gameScreenViewModel;
        
        public PointerController( GameScreenViewModel gameScreenViewModel )
        {
            _gameScreenViewModel = gameScreenViewModel ?? throw new ArgumentNullException( nameof(gameScreenViewModel) );
            
            _gameScreenViewModel.ModelView.TargetPoint.Disable();
            _gameScreenViewModel.ModelView.TargetHand.Disable();
            _gameScreenViewModel.ModelView.TargetHolder.Disable();
        }

        public void SetObservablesAround( bool isObservablesAround )
        {
            _isObservablesAround = isObservablesAround;
            SetPointer( _pointerType );
        }
        
        public void SetPointer( PointerType type )
        {
            _pointerType = type;
            
            if ( _pointerType == PointerType.None )
            {
                _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( _isObservablesAround );
                _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( false );
            }
            else if ( _pointerType == PointerType.Hand )
            {
                _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( false );
                _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( true );
            }
            else if ( _pointerType == PointerType.Point || _isObservablesAround )
            {
                _gameScreenViewModel.ModelView.TargetPoint.EnableTargetPoint( true );
                _gameScreenViewModel.ModelView.TargetHand.EnableTargetHand( false );
            }
        }
    }
}