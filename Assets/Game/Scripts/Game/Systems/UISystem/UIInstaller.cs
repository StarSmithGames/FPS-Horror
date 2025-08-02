using UnityEngine;
using Zenject;

namespace Game.UISystem
{
    [ CreateAssetMenu( fileName = "UIInstaller", menuName = "Installers/UIInstaller") ] 
    public sealed class UIInstaller : ScriptableObjectInstaller< UIInstaller >
    {
        [ SerializeField ] private UISettings _uiSettings;

        public override void InstallBindings()
        {
            Container.BindInstance( _uiSettings );
        }
    }
}