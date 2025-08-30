using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.SceneSystem
{
    public sealed class AddressableSceneProgressHandle : IProgressHandle
    {
        public bool IsDone => SceneHandle.IsValid() && SceneHandle.Status == AsyncOperationStatus.Succeeded;

        public AsyncOperationHandle< IList< IResourceLocation > > LocationHandle;
        public AsyncOperationHandle< SceneInstance > SceneHandle;
        public AsyncOperationHandle DependenciesHandle;

        public float GetProgress()
        {
            var p1 = LocationHandle.IsValid() ? LocationHandle.PercentComplete : 0f;
            var p2 = SceneHandle.IsValid() ? SceneHandle.PercentComplete : 0f;
            var p3 = DependenciesHandle.IsValid() ? DependenciesHandle.PercentComplete : 0f;
            return ( p1 + p2 + p3 ) / 3f;
        }
    }
}