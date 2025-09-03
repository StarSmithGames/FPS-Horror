using PuzzlescapeGames.Storage;
using System;

namespace Game.Systems.StorageSystem
{
    public sealed class DataHolder
    {
        public FastData FastData { get; }
        
        public GeneralStorage GeneralStorageData => _generalStorageSaveLoader.GetStorage();
        public GameStorage GameStorageData => _gameStorageSaveLoader.GetStorage();
		
        private readonly ISaveLoad< GeneralStorage > _generalStorageSaveLoader;
        private readonly ISaveLoad< GameStorage > _gameStorageSaveLoader;

        public DataHolder(
            FastData fastData,
            GeneralStorageInitializer generalStorageInitializer,
            GameStorageInitializer gameStorageInitializer
        )
        {
            FastData = fastData ?? throw new ArgumentNullException( nameof(fastData) );
            _generalStorageSaveLoader = generalStorageInitializer.GetStorage();
            _gameStorageSaveLoader = gameStorageInitializer.GetStorage();
        }

        public void SaveGeneral()
        {
            _generalStorageSaveLoader.Save();
        }
        
        public void SaveGame()
        {
            _gameStorageSaveLoader.Save();
        }
    }
}