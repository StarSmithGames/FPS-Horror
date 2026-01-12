using UnityEngine;
using Zenject;

namespace Game.Systems.InventorySystem
{
    [ CreateAssetMenu( fileName = "InventorySystemInstaller", menuName = "Installers/InventorySystemInstaller" ) ]
    public sealed class InventorySystemInstaller : ScriptableObjectInstaller< InventorySystemInstaller >
    {
        [ SerializeField ] private ItemDatabase _database;

        public override void InstallBindings()
        {
            Container.BindInstance( _database );
            Container.Bind< ItemFactory >().AsSingle().Lazy();
            Container.Bind< ItemDescriptor >().AsSingle().Lazy();
        }
    }
}