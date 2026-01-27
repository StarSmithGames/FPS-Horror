using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Entity;
using PuzzlescapeGames.Extensions;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerRightHandObject : MonoBehaviour
    {
        [Header("Idle (Realistic)")]
        [SerializeField, Range(0f, 0.05f)] private float _posAmplitudeY = 0.010f;   // дыхание по Y
        [SerializeField, Range(0f, 0.05f)] private float _posAmplitudeX = 0.004f;   // небольшой боковой ход
        [SerializeField, Range(0f, 0.05f)] private float _posAmplitudeZ = 0.002f;   // очень слабая глубина (аккуратно!)
        [SerializeField, Range(0.1f, 5f)]  private float _breathSpeed = 0.85f;      // скорость дыхания (медленно)

        [Header("Idle Rotation (Realistic)")]
        [SerializeField, Range(0f, 5f)] private float _rotX = 0.8f;
        [SerializeField, Range(0f, 5f)] private float _rotY = 0.6f;
        [SerializeField, Range(0f, 5f)] private float _rotZ = 1.2f;
        [SerializeField, Range(0.1f, 5f)] private float _rotSpeed = 0.75f;

        [Header("Idle Drift (Very Slow)")]
        [SerializeField, Range(0f, 0.03f)] private float _driftPos = 0.0035f; // медленный дрейф позиции
        [SerializeField, Range(0f, 2f)]    private float _driftRot = 0.35f;   // медленный дрейф углов
        [SerializeField, Range(0.05f, 1f)] private float _driftSpeed = 0.12f; // очень медленно

        [Header("Smoothing")]
        [SerializeField, Range(1f, 30f)] private float _posSmooth = 14f;
        [SerializeField, Range(1f, 30f)] private float _rotSmooth = 14f;

        [Header("Show / Hide")]
        [SerializeField] private Vector3 _hiddenOffset = new(0f, -0.20f, 0.10f);
        [SerializeField, Range(0.05f, 1f)] private float _showDuration = 0.18f;
        [SerializeField, Range(0.05f, 1f)] private float _hideDuration = 0.16f;
        [SerializeField] private Ease _showEase = Ease.OutCubic;
        [SerializeField] private Ease _hideEase = Ease.InCubic;

        public Transform Root => transform;

        private ItemObject _currentItem;

        private Vector3 _basePos;
        private Quaternion _baseRot;

        private bool _idleEnabled;
        private CancellationTokenSource _cancellationTokenSource;

        private float _t; // локальное время (чтобы не зависеть от Time.time скачками)
        private Vector3 _smoothedPosOffset;
        private Quaternion _smoothedRotOffset = Quaternion.identity;

        private Sequence _showHideTween;

        public void Start()
        {
            _basePos = Root.localPosition;
            _baseRot = Root.localRotation;

            PlayIdle();
        }

        public void ClearAndReset()
        {
            Root.DestroyChildren();
            
            _t = 0f;
            _smoothedPosOffset = Vector3.zero;
            _smoothedRotOffset = Quaternion.identity;

            Root.localPosition = _basePos;
            Root.localRotation = _baseRot;
        }

        public void DoAddItem( ItemObject item )
        {
            StopIdle();
            
            if ( _currentItem != null || _currentItem == item )
            {
                Hide( () =>
                {
                    ClearAndReset();

                    if ( _currentItem != null )
                    {
                        _currentItem = item;
                        _currentItem.transform.SetParent( Root );
                        _currentItem.transform.localPosition = Vector3.zero;
                        _currentItem.transform.localRotation = Quaternion.identity;

                        Show( 0.33f, PlayIdle );
                    }
                    else
                    {
                        _currentItem = null;
                    }
                });
                return;
            }
            
            _currentItem = item;
            _currentItem.transform.SetParent( Root );
            _currentItem.transform.localPosition = Vector3.zero;
            _currentItem.transform.localRotation = Quaternion.identity;

            Show( callback: PlayIdle );
        }

        public void DoRemoveItem()
        {
            if ( _currentItem == null ) return;
            
            Hide( ClearAndReset );
        }

        private void Show( float delay = 0, Action callback = null )
        {
            Root.localPosition = _basePos + _hiddenOffset;
            Root.localRotation = _baseRot;

            var startScale = Root.localScale;
            Root.localScale = startScale * 0.96f;

            _showHideTween?.Kill();
            _showHideTween = DOTween.Sequence();
            if ( delay > 0 )
            {
                _showHideTween.AppendInterval( delay );
            }
            _showHideTween
                .Append( Root.DOLocalMove( _basePos, _showDuration ).SetEase( _showEase ) )
                .Join( Root.DOScale( startScale, _showDuration ).SetEase( _showEase ) )
                .OnComplete( () => callback?.Invoke() );
        }

        private void Hide( Action callback = null )
        {
            var targetPos = _basePos + _hiddenOffset;
            var startScale = Root.localScale;

            _showHideTween?.Kill();
            _showHideTween = DOTween.Sequence()
                .Append( Root.DOLocalMove( targetPos, _hideDuration ).SetEase( _hideEase ) )
                .Join( Root.DOScale( startScale * 0.96f, _hideDuration ).SetEase( _hideEase ) )
                .OnComplete( () => callback?.Invoke() );
        }
        
        private void PlayIdle()
        {
            if ( _idleEnabled ) return;

            _idleEnabled = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            IdleLoop( _cancellationTokenSource.Token ).Forget();
        }

        private void StopIdle()
        {
            _idleEnabled = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid IdleLoop( CancellationToken token )
        {
            // чтобы не было скачка при старте
            _t = 0f;
            _smoothedPosOffset = Vector3.zero;
            _smoothedRotOffset = Quaternion.identity;

            while ( !token.IsCancellationRequested )
            {
                // если объект выключили/удалили — корректно выйдем
                if ( !this || !gameObject.activeInHierarchy )
                {
                    await UniTask.Yield( PlayerLoopTiming.Update, token );
                    continue;
                }

                float dt = Time.deltaTime;
                _t += dt;

                // --- BREATH (две частоты, чтобы не было "робота") ---
                float b1 = Mathf.Sin( _t * _breathSpeed );
                float b2 = Mathf.Sin( _t * ( _breathSpeed * 0.5f ) + 1.7f ); // фаза + другая частота

                // Позиция (очень мягко)
                Vector3 targetPosOffset = new Vector3( ( b1 * 0.65f + b2 * 0.35f ) * _posAmplitudeX, ( b1 * 0.75f + b2 * 0.25f ) * _posAmplitudeY, ( b1 * 0.60f + b2 * 0.40f ) * _posAmplitudeZ );

                // --- DRIFT (очень медленный Perlin, без "дребезга") ---
                float pnX = Mathf.PerlinNoise( _t * _driftSpeed, 11.3f ) - 0.5f;
                float pnY = Mathf.PerlinNoise( 22.7f, _t * _driftSpeed ) - 0.5f;

                targetPosOffset += new Vector3( pnX, pnY, 0f ) * _driftPos;

                // Поворот: дыхание + дрейф
                float r1 = Mathf.Sin( _t * _rotSpeed );
                float r2 = Mathf.Sin( _t * ( _rotSpeed * 0.6f ) + 2.1f );

                Vector3 targetEuler = new Vector3( ( r1 * 0.70f + r2 * 0.30f ) * _rotX, ( r1 * 0.60f + r2 * 0.40f ) * _rotY, ( r1 * 0.65f + r2 * 0.35f ) * _rotZ );

                // дрейф угла
                float pnR = ( Mathf.PerlinNoise( _t * _driftSpeed, 99.1f ) - 0.5f ) * 2f;
                targetEuler += new Vector3( pnR, -pnR * 0.6f, pnR * 0.8f ) * _driftRot;

                Quaternion targetRotOffset = Quaternion.Euler( targetEuler );

                // --- SMOOTH ---
                float posLerp = 1f - Mathf.Exp( -_posSmooth * dt );
                float rotLerp = 1f - Mathf.Exp( -_rotSmooth * dt );

                _smoothedPosOffset = Vector3.Lerp( _smoothedPosOffset, targetPosOffset, posLerp );
                _smoothedRotOffset = Quaternion.Slerp( _smoothedRotOffset, targetRotOffset, rotLerp );

                Root.localPosition = _basePos + _smoothedPosOffset;
                Root.localRotation = _baseRot * _smoothedRotOffset;

                await UniTask.Yield( PlayerLoopTiming.Update, token );
            }
        }
    }
}