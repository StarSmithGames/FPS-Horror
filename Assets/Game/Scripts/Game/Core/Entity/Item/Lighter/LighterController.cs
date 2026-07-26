using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Managers.AudioManager;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Entity
{
    public sealed class LighterController : ItemController
    {
        private readonly int Open = Animator.StringToHash( "Open" );
        private readonly int Close = Animator.StringToHash( "Close" );

        public bool IsOpened { get; private set; }
        public bool IsInProcess { get; private set; }
        public bool IsHasFlame { get; private set; }

        private float _cachedLightIntensity;
        
        private readonly Lighter _view;
        private readonly AudioManager _audioManager;

        public LighterController(
            Lighter view,
            AudioManager audioManager
            ) : base( view )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _audioManager = audioManager ?? throw new ArgumentNullException( nameof(audioManager) );
        }

        public void Initialize()
        {
            _view.Light.enabled = false;
            _view.EnableCollider( false );
            _cachedLightIntensity = _view.Light.intensity;
        }
        
        public void Show()
        {
            if ( IsInProcess ) return;
            
            OpenLighter().Forget();
        }

        public void Hide()
        {
            if ( IsInProcess ) return;
            
            CloseLighter().Forget();
        }

        private async UniTask OpenLighter()
        {
            IsInProcess = true;
            
            _view.gameObject.SetActive( true );

            if ( !IsOpened )
            {
                _view.Animator.SetTrigger( Open );
                await UniTask.WaitForSeconds( 0.05f );
                _audioManager.PlaySound( _view.SoundOpen );
            
                IsOpened = true;
                
                await UniTask.WaitForSeconds( 0.48f );
            }

            if ( !IsHasFlame )
            {
                _audioManager.PlaySound( _view.SoundIgnite );

                await UniTask.WaitForSeconds( 0.08f );
                _view.Flame.transform.localScale = Vector3.zero;
                _view.Flame.transform.DOScale( 1f, 0.16f );
                _view.Flame.Play();
                _view.Light.intensity = 0f;
                _view.Light.DOIntensity( _cachedLightIntensity, 0.16f );
                _view.Light.enabled = true;
                
                IsHasFlame = true;
                
                await UniTask.WaitForSeconds( 0.16f );
            }
            
            IsInProcess = false;
        }

        private async UniTask CloseLighter()
        {
            IsInProcess = true;
            
            _view.Animator.SetTrigger( Close );
            _view.Flame.transform.DOScale( 0f, 0.16f );
            _view.Light.DOIntensity( 0f, 0.16f );
            await UniTask.WaitForSeconds( 0.05f );
            _audioManager.PlaySound( _view.SoundClose );
            
            IsOpened = false;

            if ( IsHasFlame )
            {
                await UniTask.WaitForSeconds( 0.16f - 0.05f );

                _view.Flame.Stop();
                _view.Light.enabled = false;
                
                IsHasFlame = false;
            }
            
            await UniTask.WaitForSeconds( 0.16f );
            
            _view.gameObject.SetActive( false );
            
            IsInProcess = false;
        }
    }
}