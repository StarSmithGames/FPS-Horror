using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Entity;
using Game.Managers.InputManager;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core.World.InspectionSystem
{
    public sealed class InspectionSystem
    {
        private const float MOUSE_ROTATION_SENSITIVITY = 0.2f;
        private const float GAMEPAD_ROTATION_SPEED = 120f;
        private const float GAMEPAD_DEAD_ZONE = 0.1f;
        private const float ROTATION_SMOOTHNESS = 18f;
        private const float POSITION_SMOOTHNESS = 12f;
        
        private ItemObject _inspectableItem;
        private CancellationTokenSource _cancellationTokenSource;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private bool _isBlocked;
        
        private readonly Camera _camera;

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
            StopInspection();
            _inspectableItem = item;
            var itemTransform = _inspectableItem.TransformInspection;
            itemTransform.DOKill();

            _inspectableItem.EnableCollider( false );
            _originalPosition = itemTransform.position;
            _originalRotation = itemTransform.rotation;

            InputManager.Inputs.UI.Inspection.Enable();
            InputManager.Inputs.UI.Click.Enable();

            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
        }

        public void StopInspection()
        {
            if ( _inspectableItem == null )
            {
                return;
            }

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            InputManager.Inputs.UI.Inspection.Disable();
            InputManager.Inputs.UI.Click.Disable();

            _inspectableItem.TransformInspection.DOKill();
            AnimateOut().Forget();
            
            _inspectableItem = null;
        }

        private async UniTask Tick( CancellationToken cancellationToken )
        {
            Quaternion alignedRotation = CalculateAlignedRotation( _inspectableItem );
            
            await AnimateIn( alignedRotation, cancellationToken );

            Vector2 currentRotation = Vector2.zero;
            Vector2 targetRotation = Vector2.zero;
            var settings = _inspectableItem.InspectionSettings;
            var itemTransform = _inspectableItem.TransformInspection;

            while ( !cancellationToken.IsCancellationRequested )
            {
                float deltaTime = Time.unscaledDeltaTime;

                Vector3 localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
                Vector3 targetPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;
                float positionLerp = 1f - Mathf.Exp( -POSITION_SMOOTHNESS * deltaTime );
                itemTransform.position = Vector3.Lerp( itemTransform.position, targetPosition, positionLerp );

                Quaternion userRotation = Quaternion.identity;

                if ( settings.IsRotatable )
                {
                    if ( !_isBlocked )
                    {
                        var inspectionAction = InputManager.Inputs.UI.Inspection;
                        Vector2 actionInput = inspectionAction.ReadValue< Vector2 >();
                        Vector2 gamepadInput = Gamepad.current?.rightStick.ReadValue() ?? Vector2.zero;

                        // Если геймпад уже привязан к Inspection, используем значение экшена.
                        if ( inspectionAction.activeControl?.device is Gamepad )
                        {
                            gamepadInput = actionInput;
                        }

                        float yDirection = settings.IsInverseY ? -1f : 1f;

                        if ( gamepadInput.sqrMagnitude >= GAMEPAD_DEAD_ZONE * GAMEPAD_DEAD_ZONE )
                        {
                            // Стик задаёт скорость поворота, поэтому здесь нужен deltaTime.
                            gamepadInput.y *= yDirection;
                            targetRotation += gamepadInput * GAMEPAD_ROTATION_SPEED * deltaTime;
                        }
                        else if ( InputManager.Inputs.UI.Click.IsPressed() )
                        {
                            // Mouse delta уже является смещением за кадр — deltaTime здесь не нужен.
                            actionInput.y *= yDirection;
                            targetRotation += actionInput * MOUSE_ROTATION_SENSITIVITY;
                        }
                    }

                    float rotationLerp = 1f - Mathf.Exp( -ROTATION_SMOOTHNESS * deltaTime );
                    currentRotation = Vector2.Lerp( currentRotation, targetRotation, rotationLerp );
                    userRotation = Quaternion.Euler( -currentRotation.y, -currentRotation.x, 0f );
                }
                itemTransform.rotation = alignedRotation * userRotation * Quaternion.Euler( settings.RotationOffset );

                await UniTask.Yield( cancellationToken );
            }
        }

        private Quaternion CalculateAlignedRotation( ItemObject item )
        {
            Transform itemTransform = item.TransformInspection;
            var settings = item.InspectionSettings;

            Quaternion sourceRotation = itemTransform.rotation;
            Vector3 itemFaceWorld = itemTransform.TransformDirection( settings.FaceAxis.ToDirection() );
            Vector3 itemBottomWorld = itemTransform.TransformDirection( settings.BottomAxis.ToDirection() );

            Quaternion alignFace = Quaternion.FromToRotation( itemFaceWorld, -_camera.transform.forward );
            Vector3 rotatedBottom = alignFace * itemBottomWorld;
            Quaternion alignBottom = Quaternion.FromToRotation( rotatedBottom, -_camera.transform.up );

            return alignBottom * alignFace * sourceRotation;
        }

        private async UniTask AnimateIn( Quaternion alignedRotation, CancellationToken cancellationToken )
        {
            var itemTransform = _inspectableItem.TransformInspection;
            var settings = _inspectableItem.InspectionSettings;

            itemTransform.DOKill();

            Vector3 localOffset = _camera.transform.TransformDirection( settings.PositionOffset );
            Vector3 finalPosition = _camera.transform.position + _camera.transform.forward * 0.5f + localOffset;
            Quaternion finalRotation = alignedRotation * Quaternion.Euler( settings.RotationOffset );

            UniTask moveTask = itemTransform.DOMove( finalPosition, 0.33f ).SetEase( Ease.OutCubic ).SetUpdate( true ).ToUniTask( cancellationToken: cancellationToken );
            UniTask rotateTask = itemTransform.DORotateQuaternion( finalRotation, 0.33f ).SetEase( Ease.OutCubic ).SetUpdate( true ).ToUniTask( cancellationToken: cancellationToken );

            await UniTask.WhenAll( moveTask, rotateTask );
        }

        private async UniTask AnimateOut()
        {
            var item = _inspectableItem;
            var itemTransform = item.TransformInspection;
            itemTransform.DOKill();

            UniTask moveTask = itemTransform.DOMove( _originalPosition, 0.33f ).SetEase( Ease.InCubic ).SetUpdate( true ).ToUniTask();
            UniTask rotateTask = itemTransform.DORotateQuaternion( _originalRotation, 0.33f ).SetEase( Ease.InCubic ).SetUpdate( true ).ToUniTask();

            await UniTask.WhenAll( moveTask, rotateTask );
            item.EnableCollider( true );
        }
    }
}