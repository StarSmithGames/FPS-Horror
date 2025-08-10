using UnityEngine;
using Zenject;

namespace Game.Managers.InputManager
{
    [ CreateAssetMenu( fileName = "InputInstaller", menuName = "Installers/InputInstaller" ) ]
    public sealed class InputInstaller : ScriptableObjectInstaller< InputInstaller >
    {
        [ SerializeField ] private InputKeyActionsSettings _settings;
        
        public override void InstallBindings()
        {
            Container.BindInstance( _settings );
        }
    }
}