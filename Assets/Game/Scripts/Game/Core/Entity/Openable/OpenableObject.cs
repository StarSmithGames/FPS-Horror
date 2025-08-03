using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class OpenableObject : DynamicObject, IOpenable
    {
        [ SerializeField ] private Animation _animation;
        [ SerializeField ] private OpenCloseSettings _openCloseSettings;

        public void Open()
        {
            _animation[ _openCloseSettings.OpenAnimationName ].speed = _openCloseSettings.OpenSpeed;
            _animation[ _openCloseSettings.OpenAnimationName ].normalizedTime = 0;
            _animation.Play( _openCloseSettings.OpenAnimationName );
        }

        public void Close()
        {
            if ( _openCloseSettings.IsCloseAnimationIsOpenReverse )
            {
                _animation[ _openCloseSettings.OpenAnimationName ].speed = -1 * _openCloseSettings.CloseSpeed;
                if ( _animation[ _openCloseSettings.OpenAnimationName ].normalizedTime > 0 )
                {
                    _animation[ _openCloseSettings.OpenAnimationName ].normalizedTime = _animation[ _openCloseSettings.OpenAnimationName ].normalizedTime;
                }
                else
                {
                    _animation[ _openCloseSettings.OpenAnimationName ].normalizedTime = 1;
                }
                _animation.Play( _openCloseSettings.OpenAnimationName );
            }
            else
            {
                _animation[ _openCloseSettings.CloseAnimationName ].speed = _openCloseSettings.CloseSpeed;
                _animation[ _openCloseSettings.CloseAnimationName ].normalizedTime = 0;
                _animation.Play( _openCloseSettings.CloseAnimationName );
            }
        }
    }
}