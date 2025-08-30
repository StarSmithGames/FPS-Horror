using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

namespace Game.SceneSystem
{
    public sealed class SceneSystem : ISceneSystem
    {
        public IProgressHandle Handle { get; private set; }
        
        private Scene _currentScene;

        public SceneSystem()
        {
            _currentScene = GetActiveScene();
        }
        
        public Scene GetActiveScene()
        {
            return SceneManager.GetActiveScene();
        }

        public void LoadSceneForce( int scene, LoadSceneMode mode = LoadSceneMode.Single )
        {
            SceneManager.LoadScene( scene, mode );
        }

        public async UniTask LoadSceneFromBuild( string sceneName, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single )
        {
            var scene = SceneManager.GetSceneByName( sceneName );
            await LoadFromBuild( scene.buildIndex, allow, mode );
        }

        public async UniTask LoadSceneFromBuild( int sceneBuildIndex, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single )
        {
            await LoadFromBuild( sceneBuildIndex, allow, mode );
        }

        private async UniTask LoadFromBuild( int sceneBuildIndex, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single )
        {
            var handle = new SceneProgressHandle()
            {
                asyncOperation = SceneManager.LoadSceneAsync( sceneBuildIndex, mode )
            };
            Handle = handle;
            
            
            handle.asyncOperation.allowSceneActivation = allow;
            await handle.asyncOperation;

            if ( handle.asyncOperation.isDone )
            {
                _currentScene = GetActiveScene();
            }
            else
            {
                throw new SceneNoLoadedException( $"[{GetType().Name}] Can not load scene {sceneBuildIndex}!" );
            }
        }
        
        #region Load From Addressables
        
        private Dictionary<string, IResourceLocation> _resourceLocations = new();

        public async UniTask LoadSceneFromAddressables( string locationKey, string addressableLocationKey, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single )
        {
            await LoadFromAddressables( locationKey, addressableLocationKey, allow, mode );
        }
        
        private async UniTask LoadFromAddressables( string locationKey, string addressableLocationKey, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single )
        {
            AddressableSceneProgressHandle handle = new();
            Handle = handle;
            
            IResourceLocation location = null;
            await LoadLocation(locationKey, addressableLocationKey, (x) =>
            {
                location = x;
            });
            
            if (location != null)
            {
	            Debug.Log($"[{GetType().Name}] Start DownloadDependenciesAsync {addressableLocationKey}");
	            handle.DependenciesHandle = Addressables.DownloadDependenciesAsync( addressableLocationKey );
	            await handle.DependenciesHandle;
	            Debug.Log($"[{GetType().Name}] End DownloadDependenciesAsync");
				
                Debug.Log($"[{GetType().Name}] Start LoadSceneAsync {location}");
                handle.SceneHandle = Addressables.LoadSceneAsync( location );
                await handle.SceneHandle;
                Debug.Log($"[{GetType().Name}] End LoadSceneAsync");

                if (handle.SceneHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentScene = GetActiveScene();

                    if ( !allow )
                    {
                        await handle.SceneHandle.Result.ActivateAsync();
                    }
                }
                else if (handle.SceneHandle.Status == AsyncOperationStatus.Failed)
                {
                    throw new SceneNoLoadedException($"[{GetType().Name}] Can't load location by key: {locationKey} with addressable: {addressableLocationKey} Exception: {handle.SceneHandle.OperationException}");
                }
                else
                {
                    throw new SceneNoLoadedException($"[{GetType().Name}] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}, incorrect status");
                }
            }
            else
            {
                throw new SceneNoLoadedException($"[{GetType().Name}] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}, location == NULL");
            }
        }

        private async UniTask LoadLocation( string locationKey, string addressableLocationKey, Action< IResourceLocation > callback )
        {
            if ( !_resourceLocations.TryGetValue( locationKey, out var location ) )
            {
                var addressablesHandle = Handle as AddressableSceneProgressHandle;

                addressablesHandle.LocationHandle = Addressables.LoadResourceLocationsAsync( addressableLocationKey );
                await addressablesHandle.LocationHandle;

                if ( addressablesHandle.LocationHandle.IsDone && addressablesHandle.LocationHandle.Status == AsyncOperationStatus.Succeeded && addressablesHandle.LocationHandle.Result.Count > 0 )
                {
                    location = addressablesHandle.LocationHandle.Result[ 0 ];
                    if ( !_resourceLocations.ContainsKey( locationKey ) )
                    {
                        _resourceLocations.Add( locationKey, location );
                    }
                    else
                    {
                        _resourceLocations[ locationKey ] = location;
                    }

                }
            }

            if ( location == null )
            {
                throw new SceneNoLoadedException( $"[{GetType().Name}] Can't load location by key: {locationKey} with addressable: {addressableLocationKey}" );
            }

            callback.Invoke( location );
        }

        #endregion
    }
}