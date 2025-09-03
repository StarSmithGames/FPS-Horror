using Zenject;

namespace Game.Systems.StorageSystem
{
    public sealed class StorageSystemInstaller : Installer< StorageSystemInstaller >
    {
        public override void InstallBindings()
        {
            Container.Bind< FastData >().AsSingle();

            Container.Bind< GeneralStorageInitializer >().AsSingle().WhenInjectedInto< DataHolder >();
            Container.Bind< GameStorageInitializer >().AsSingle().WhenInjectedInto< DataHolder >();
            Container.Bind< DataHolder >().AsSingle().NonLazy();
        }
    }
}