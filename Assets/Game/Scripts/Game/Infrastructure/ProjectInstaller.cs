using PuzzlescapeGames.IoC;
using Zenject;

namespace Game
{
    public sealed class ProjectInstaller : MonoInstaller< ProjectInstaller >
    {
        public override void InstallBindings()
        {
            DiManager.CurrentContainer = Container;
        }
    }
}