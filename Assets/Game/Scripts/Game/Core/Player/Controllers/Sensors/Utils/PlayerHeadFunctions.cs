using Game.Core.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerHeadFunctions
    {
        private Transform _head;
        private Vector3 _lastHitPoint;
        
        private readonly PlayerConfig _config;
        private readonly PlayerObject _view;
        
        public PlayerHeadFunctions(
            PlayerConfig config,
            PlayerObject view
            )
        {
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            
            _head = _view.CameraFPS.transform;
        }
        
        public bool CastInFrontRay( out RaycastHit hit )
        {
            Ray ray = new Ray( _head.position, _head.forward );
            if ( Physics.Raycast( ray, out hit, _config.CameraVisionSettings.MaxRayDistance, _config.CameraVisionSettings.DefaultLayers ) )
            {
                _lastHitPoint = hit.point;

                Collider[] collidersIntersects = Physics.OverlapSphere( _lastHitPoint, _config.CameraVisionSettings.SphereRadius, _config.CameraVisionSettings.InteractLayers );
                return collidersIntersects.Length > 0;
            }

            return false;
        }
        
        public InteractableObject GetInFrontTarget( Vector3 hitPoint )
        {
            var colliders = Physics.OverlapSphere( hitPoint, 0.025f, _config.CameraVisionSettings.InteractLayers );
            if ( colliders is { Length: > 0 } )
            {
                for ( int i = 0; i < colliders.Length; i++ )
                {
                    var observable = colliders[ i ].transform.GetComponentInParent< InteractableObject >();
                    if ( observable != null )
                    {
                        return observable;
                    }
                }
            }

            return null;
        }

        public List< InteractableObject > GetTargetsAround()
        {
            Collider[] colliders = Physics.OverlapSphere( _head.position, _config.InteractionsSettings.MaxDistance, _config.CameraVisionSettings.InteractLayers );
            return GetInteractables( colliders );
        }
        
        private List< InteractableObject > GetInteractables( Collider[] colliders )
        {
            List< InteractableObject > result = new();

            foreach ( var collider in colliders )
            {
                var interactable = collider.GetComponentInParent< InteractableObject >();
                if ( interactable == null) continue;
                if ( !interactable.IsCollidersEnabled ) continue;
                if ( !IsInFOV( interactable.GetInteractPointerPosition() ) ) continue;
                
                result.Add( interactable );
            }

            return result;
        }

        public InteractableObject FindNearestInteractable( List< InteractableObject > targets )
        {
            if ( targets.Count > 0 )
            {
                InteractableObject nearest = null;
                float minSqrDistance = _config.InteractionsSettings.KeyDistanceSquared;

                foreach ( var target in targets )
                {
                    Vector3 closestPoint = target.GetInteractPointerPosition();
                    float sqrDist = ( closestPoint - _head.position ).sqrMagnitude;

                    if ( sqrDist < minSqrDistance )
                    {
                        minSqrDistance = sqrDist;
                        nearest = target;
                    }
                }

                return nearest;
            }

            return null;
        }

        public InteractableObject FindBestKeyInteractable( List< InteractableObject > targets )
        {
            if ( targets == null || targets.Count == 0 )
                return null;

            // Насколько "примерно смотрим" на объект.
            // 12° = довольно строго (почти по центру). Если надо мягче — поставь 18-25.
            const float keyConeAngleDeg = 15f;
            float minDotKey = Mathf.Cos( keyConeAngleDeg * Mathf.Deg2Rad );

            InteractableObject best = null;
            float bestDot = -1f;
            float bestSqrDist = float.MaxValue;

            for ( int i = 0; i < targets.Count; i++ )
            {
                var t = targets[ i ];
                if ( t == null ) continue;
                if ( !t.IsCollidersEnabled ) continue;

                Vector3 p = t.GetInteractPointerPosition();
                Vector3 to = p - _head.position;

                float sqrDist = to.sqrMagnitude;

                // Key только в радиусе "KeyDistance"
                if ( sqrDist > _config.InteractionsSettings.KeyDistanceSquared )
                    continue;

                Vector3 dir = to.normalized;
                float dot = Vector3.Dot( _head.forward, dir );

                // Должен быть примерно по центру взгляда
                if ( dot < minDotKey )
                    continue;

                // Выбираем максимально "в центре" (dot), при равенстве — ближе
                if ( dot > bestDot || ( Mathf.Approximately( dot, bestDot ) && sqrDist < bestSqrDist ) )
                {
                    bestDot = dot;
                    bestSqrDist = sqrDist;
                    best = t;
                }
            }

            return best;
        }

        private bool IsInFOV( Vector3 worldPoint )
        {
            Vector3 dirToTarget = ( worldPoint - _head.position ).normalized;

            float dot = Vector3.Dot( _head.forward, dirToTarget );
            float minDot = Mathf.Cos( _view.CameraFPS.fieldOfView * 0.5f * Mathf.Deg2Rad );

            return dot >= minDot;
        }
    }
}