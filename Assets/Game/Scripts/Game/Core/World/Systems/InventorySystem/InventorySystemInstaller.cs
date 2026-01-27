using Game.Core.World.InventorySystem.ContextMenu;
using UnityEngine;
using Zenject;

namespace Game.Core.World.InventorySystem
{
    [ CreateAssetMenu( fileName = "InventorySystemInstaller", menuName = "Installers/InventorySystemInstaller" ) ]
    public sealed class InventorySystemInstaller : ScriptableObjectInstaller< InventorySystemInstaller >
    {
        [ SerializeField ] private ItemDatabase _database;
        [ SerializeField ] private ContextMenuItems _contextMenuItems;

        public override void InstallBindings()
        {
            Container.BindInstance( _database );
            Container.Bind< ItemFactory >().AsSingle().Lazy();
            Container.Bind< ItemDescriptor >().AsSingle().Lazy();
            Container.Bind< ItemCommander >().AsSingle().Lazy();
            
            Container.BindInstance( _contextMenuItems );
            Container.Bind< ContextMenuService >().AsSingle().Lazy();
        }
    }
}