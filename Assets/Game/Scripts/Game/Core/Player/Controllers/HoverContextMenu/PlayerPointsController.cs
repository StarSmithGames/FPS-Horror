using Game.Core.Entity;
using Game.Core.World.PointerSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerPointsController
    {
        private InteractionPointerDictionary _dictionary = new();
        
        private readonly InteractionPointerFactory _interactionPointerFactory;
        private readonly PlayerConfig _config;
        
        public PlayerPointsController(
            InteractionPointerFactory interactionPointerFactory,
            PlayerConfig config
            )
        {
            _interactionPointerFactory = interactionPointerFactory ?? throw new ArgumentNullException( nameof(interactionPointerFactory) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
        }

        public void PointsAround( List< InteractableObject > allTargets, Transform root, Transform camera )
        {
            _dictionary.TryRemoveSubtractions( allTargets );
            
            for ( int i = 0; i < allTargets.Count; i++ )
            {
                var target = allTargets[ i ];
                if ( !target.IsCollidersEnabled )
                {
                    _dictionary.TryRemove( target );
                    continue;
                }
                
                Pointing( target, root, camera );
            }
        }

        private void Pointing( InteractableObject target, Transform root, Transform camera )
        {
            Vector3 delta = target.PointerStartPosition - root.position;
            delta.y = 0f;
            float sqrMagnitude = delta.sqrMagnitude;
            if ( sqrMagnitude < _config.InteractionsSettings.MaxDistanceSquared )//if root is close enough
            {
                if ( _dictionary.IsShowing( target ) )
                {
                    var pointer = _dictionary.Get( target );
                    if ( sqrMagnitude < _config.InteractionsSettings.KeyDistanceSquared )
                    {
                        pointer.HidePointShowKey();
                    }
                    else
                    {
                        pointer.ShowPointHideKey();
                    }
                    
                    return;
                }
                        
                //show pointer
                if ( !_dictionary.Contains( target ) )
                {
                    var pointer = _interactionPointerFactory.Create();
                    _dictionary.TryAdd( target, pointer );
                    
                    pointer.StartLookAt( camera, target );
                    pointer.Show();
                    if ( sqrMagnitude < _config.InteractionsSettings.KeyDistanceSquared )
                    {
                        pointer.HidePointShowKey();
                    }
                    else
                    {
                        pointer.ShowPointHideKey();
                    }
                }
            }
            else
            {
                if ( _dictionary.IsShowing( target ) )
                {
                    _dictionary.TryRemove( target );
                }
            }
        }
    }
}