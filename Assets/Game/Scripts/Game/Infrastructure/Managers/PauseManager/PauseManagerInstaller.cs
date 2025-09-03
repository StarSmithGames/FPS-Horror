using Zenject;

namespace Game.Managers.PauseManager
{
    public sealed class PauseManagerInstaller : Installer< PauseManagerInstaller >
    {
        public override void InstallBindings()
        {
            Container.Bind< PauseManager >().AsSingle();
        }
    }
}