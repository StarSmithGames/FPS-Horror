using UnityEngine;
using Zenject;

namespace Game.StoryFlow
{
    [ CreateAssetMenu( fileName = "StoryFlowInstaller", menuName =  "Installers/StoryFlowInstaller") ]
    public sealed class StoryFlowInstaller : ScriptableObjectInstaller< StoryFlowInstaller >
    {
        public override void InstallBindings()
        {
            Container.Bind< StoryManager >().AsSingle();
        }
    }
}