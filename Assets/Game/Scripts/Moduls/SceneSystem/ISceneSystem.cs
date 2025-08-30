using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.SceneSystem
{
    public interface ISceneSystem
    {
        IProgressHandle Handle { get; }

        Scene GetActiveScene();

        void LoadSceneForce( int scene, LoadSceneMode mode = LoadSceneMode.Single );
        UniTask LoadSceneFromBuild( string sceneName, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single );
        UniTask LoadSceneFromBuild( int sceneBuildIndex, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single );

        UniTask LoadSceneFromAddressables( string locationKey, string addressableLocationKey, bool allow = true, LoadSceneMode mode = LoadSceneMode.Single );
    }
}