using Zenject;

namespace Game.SceneSystem
{
    public sealed class SceneSystemInstaller : Installer< SceneSystemInstaller >
    {
        public override void InstallBindings()
        {
            Container.Bind< ISceneSystem >().To< SceneSystem >().AsSingle();
        }
    }
}