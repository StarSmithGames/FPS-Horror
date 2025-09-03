using PuzzlescapeGames.Storage;
using System;

namespace Game.Systems.StorageSystem
{
    public sealed class GameStorageInitializer
    {
        private readonly FastData _fastData;

        public GameStorageInitializer( FastData fastData )
        {
            _fastData = fastData ?? throw new ArgumentNullException( nameof(fastData) );
        }

        public ISaveLoad< GameStorage > GetStorage()
        {
            ISaveLoad< GameStorage > storageSaveLoader = new PlayerPrefsSaveLoad< GameStorage >( "game_storage" );
            GameStorage storage = storageSaveLoader.GetStorage();
            
            if ( _fastData.IsFirstTime )
            {
                AddFirstTimeData( storage );
            }
            
            var isSupported = AddDataForOldPlayers(storage);
            var isPurged = PurgeOldKeys(storage);
            
            if ( _fastData.IsFirstTime || isSupported || isPurged )
            {
                storageSaveLoader.Save();
            }

            return storageSaveLoader;
        }
        
        private void AddFirstTimeData( GameStorage storage )
        {
            // storage.GameProgress.SetData( new GameProgressData()
            // {
            //     RegularLevelIndex = 0,
            //     RegularLevels = new( _gameplayConfig.RegularLocalLevels.Count + _gameplayConfig.RegularRemoteLevels.Count )
            // } );
            // storage.HintBooster.SetData( new BoosterData() { Count = 3 } );
        }

        private bool AddDataForOldPlayers( GameStorage storage )
        {
            var isSupported = false;

            return isSupported;
        }
        
        private bool PurgeOldKeys( GameStorage storage )
        {
            var isPurged = false;

            return isPurged;
        }
    }
}