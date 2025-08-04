using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Entity;
using Game.Managers.InputManager;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.World.InspectionSystem
{
    public sealed class InspectionSystem
    {
        private const float rotationSpeed = 50f;
        private const float verticalClampTop = 40f;
        private const float verticalClampBottom = -80f;

        private bool _isBlocked;
        
        private ItemObject _inspectableItem;
        private CancellationTokenSource _cancellationTokenSource;
        private Camera _camera;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Quaternion _initialAlignedRotation;
        private Vector3 _lastMousePosition;
        
        public InspectionSystem( Camera camera )
        {
            _camera = camera ?? throw new ArgumentNullException( nameof(camera) );
        }

        public void Block( bool trigger )
        {
            _isBlocked = trigger;
        }

        public void StartInspection( ItemObject item )
        {
            _inspectableItem = item ?? throw new ArgumentNullException( nameof(item) );

            _inspectableItem.EnableCollider( false );
            _originalPosition = _inspectableItem.TransformInspection.position;
            _originalRotation = _inspectableItem.TransformInspection.rotation;
            
            Vector3 itemFaceWorld = _inspectableItem.TransformInspection.TransformDirection( _inspectableItem.InspectionSettings.FaceAxis.ToDirection() );
            Vector3 itemBottomWorld = _inspectableItem.TransformInspection.TransformDirection( _inspectableItem.InspectionSettings.BottomAxis.ToDirection() );

            Vector3 desiredUp = ( _camera.transform.position - _inspectableItem.TransformInspection.position ).normalized;
            Vector3 desiredForward = -_camera.transform.up;

            Quaternion alignUp = Quaternion.FromToRotation( itemFaceWorld, desiredUp );
            Vector3 rotatedForward = alignUp * itemBottomWorld;
            Quaternion alignForward = Quaternion.FromToRotation( rotatedForward, desiredForward );
            
            _initialAlignedRotation = alignForward * alignUp;

            _cancellationTokenSource = new();
            InputManager.Inputs.UI.Inspection.Enable();
            InputManager.Inputs.UI.Click.Enable();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public void StopInspection()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            InputManager.Inputs.UI.Inspection.Disable();
            InputManager.Inputs.UI.Click.Disable();
            
            AnimateOut().Forget();
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            await AnimateIn( cancellationToken );
            
            Vector2 currentRotation = Vector2.zero;
            Vector2 targetRotation = Vector2.zero;
            var settings = _inspectableItem.InspectionSettings;

            while ( !cancellationToken.IsCancellationRequested )
            {
                //Центрируем предмет перед камерой
                var localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
                var targetPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;
                _inspectableItem.TransformInspection.position = Vector3.Lerp( _inspectableItem.TransformInspection.position, targetPosition, 0.05f );

                Quaternion userRotation = Quaternion.identity;
                if ( settings.IsRotatable && !_isBlocked )
                {
                    Vector2 input = InputManager.Inputs.UI.Inspection.ReadValue< Vector2 >();
                    if ( InputManager.Inputs.UI.Click.IsPressed() )
                    {
                        targetRotation += input * rotationSpeed * Time.deltaTime;
                    }

                    // currentRotation = Vector2.Lerp( currentRotation, targetRotation, rotationSpeed * Time.deltaTime );
                    currentRotation = targetRotation;
                    currentRotation.y = Mathf.Clamp( currentRotation.y, verticalClampBottom, verticalClampTop );

                    userRotation = Quaternion.Euler( -currentRotation.y, -currentRotation.x, 0f );
                }

                _inspectableItem.TransformInspection.rotation = _initialAlignedRotation * userRotation * Quaternion.Euler( settings.RotationOffset );

                await UniTask.Yield();
            }
        }

        private async UniTask AnimateIn( CancellationToken cancellationToken = default )
        {
            var settings = _inspectableItem.InspectionSettings;

            Vector3 itemFaceWorld = _inspectableItem.TransformInspection.TransformDirection( settings.FaceAxis.ToDirection() );
            Vector3 itemBottomWorld = _inspectableItem.TransformInspection.TransformDirection( settings.BottomAxis.ToDirection() );

            Vector3 desiredUp = ( _camera.transform.position - _inspectableItem.TransformInspection.position ).normalized;
            Vector3 desiredForward = -_camera.transform.up;

            Quaternion alignUp = Quaternion.FromToRotation( itemFaceWorld, desiredUp );
            Vector3 rotatedForward = alignUp * itemBottomWorld;
            Quaternion alignForward = Quaternion.FromToRotation( rotatedForward, desiredForward );

            Quaternion baseRotation = alignForward * alignUp;
            _initialAlignedRotation = baseRotation;

            Vector3 localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
            Vector3 finalPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;

            _inspectableItem.TransformInspection.DOMove( finalPosition, 0.33f ).SetEase( Ease.OutCubic ).ToUniTask( cancellationToken: cancellationToken );
            await UniTask.WaitForSeconds( 0.08f, cancellationToken: cancellationToken );
            await _inspectableItem.TransformInspection
                .DORotateQuaternion( baseRotation * Quaternion.Euler( settings.RotationOffset ), 0.33f )
                .SetEase( Ease.OutCubic )
                .ToUniTask( cancellationToken: cancellationToken );
            await UniTask.Yield();
        }

        private async UniTask AnimateOut()
        {
            _inspectableItem.TransformInspection.DOMove( _originalPosition, 0.33f ).SetEase( Ease.InCubic ).ToUniTask();
            await _inspectableItem.TransformInspection.DORotateQuaternion( _originalRotation, 0.33f ).SetEase( Ease.InCubic ).ToUniTask();
            
            _inspectableItem.EnableCollider( true );
        }
    }
}