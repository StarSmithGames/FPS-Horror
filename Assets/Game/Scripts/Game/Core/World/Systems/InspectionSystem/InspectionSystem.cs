using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        private IInspectable _inspectable;
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

        public void StartInspection( IInspectable inspectable )
        {
            _inspectable = inspectable ?? throw new ArgumentNullException( nameof(inspectable) );

            _inspectable.EnableCollider( false );
            _originalPosition = _inspectable.TransformInspection.position;
            _originalRotation = _inspectable.TransformInspection.rotation;

            _cancellationTokenSource = new();
            InputManager.Inputs.UI.Inspection.Enable();
            InputManager.Inputs.UI.Click.Enable();

            // --- вычисление начального выравнивания
            var settings = _inspectable.InspectionSettings;

            Vector3 itemFaceWorld = _inspectable.TransformInspection.TransformDirection( settings.FaceAxis.ToDirection() );
            Vector3 itemBottomWorld = _inspectable.TransformInspection.TransformDirection( settings.BottomAxis.ToDirection() );

            Vector3 desiredUp = ( _camera.transform.position - _inspectable.TransformInspection.position ).normalized;
            Vector3 desiredForward = -_camera.transform.up;

            Quaternion alignUp = Quaternion.FromToRotation( itemFaceWorld, desiredUp );
            Vector3 rotatedForward = alignUp * itemBottomWorld;
            Quaternion alignForward = Quaternion.FromToRotation( rotatedForward, desiredForward );
            
            _initialAlignedRotation = alignForward * alignUp;

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
            var settings = _inspectable.InspectionSettings;

            while ( !cancellationToken.IsCancellationRequested )
            {
                //Центрируем предмет перед камерой
                var localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
                var targetPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;
                _inspectable.TransformInspection.position = Vector3.Lerp( _inspectable.TransformInspection.position, targetPosition, 0.05f );

                Quaternion userRotation = Quaternion.identity;
                if ( settings.IsRotatable )
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

                _inspectable.TransformInspection.rotation = _initialAlignedRotation * userRotation * Quaternion.Euler( settings.RotationOffset );

                await UniTask.Yield();
            }
        }

        private async UniTask AnimateIn( CancellationToken cancellationToken = default )
        {
            var settings = _inspectable.InspectionSettings;

            Vector3 itemFaceWorld = _inspectable.TransformInspection.TransformDirection( settings.FaceAxis.ToDirection() );
            Vector3 itemBottomWorld = _inspectable.TransformInspection.TransformDirection( settings.BottomAxis.ToDirection() );

            Vector3 desiredUp = ( _camera.transform.position - _inspectable.TransformInspection.position ).normalized;
            Vector3 desiredForward = -_camera.transform.up;

            Quaternion alignUp = Quaternion.FromToRotation( itemFaceWorld, desiredUp );
            Vector3 rotatedForward = alignUp * itemBottomWorld;
            Quaternion alignForward = Quaternion.FromToRotation( rotatedForward, desiredForward );

            Quaternion baseRotation = alignForward * alignUp;
            _initialAlignedRotation = baseRotation;

            Vector3 localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
            Vector3 finalPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;

            _inspectable.TransformInspection.DOMove( finalPosition, 0.33f ).SetEase( Ease.OutCubic ).ToUniTask( cancellationToken: cancellationToken );
            await UniTask.WaitForSeconds( 0.08f, cancellationToken: cancellationToken );
            await _inspectable.TransformInspection
                .DORotateQuaternion( baseRotation * Quaternion.Euler( settings.RotationOffset ), 0.33f )
                .SetEase( Ease.OutCubic )
                .ToUniTask( cancellationToken: cancellationToken );
            await UniTask.Yield();
        }

        private async UniTask AnimateOut()
        {
            _inspectable.TransformInspection.DOMove( _originalPosition, 0.33f ).SetEase( Ease.InCubic ).ToUniTask();
            await _inspectable.TransformInspection.DORotateQuaternion( _originalRotation, 0.33f ).SetEase( Ease.InCubic ).ToUniTask();
            
            _inspectable.EnableCollider( true );
        }
    }
}