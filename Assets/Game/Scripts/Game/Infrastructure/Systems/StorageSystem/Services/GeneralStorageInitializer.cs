using PuzzlescapeGames.Storage;
using System;
using UnityEngine;

namespace Game.Systems.StorageSystem
{
    public sealed class GeneralStorageInitializer
    {
        private readonly FastData _fastData;

        public GeneralStorageInitializer( FastData fastData )
        {
            _fastData = fastData ?? throw new ArgumentNullException( nameof(fastData) );
        }

        public ISaveLoad< GeneralStorage > GetStorage()
        {
            ISaveLoad< GeneralStorage > storageSaveLoader = new PlayerPrefsSaveLoad< GeneralStorage >( "general_storage" );
            GeneralStorage storage = storageSaveLoader.GetStorage();
            
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
        
        private void AddFirstTimeData( GeneralStorage storage )
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            storage.Graphics.SetData( new GraphicsData()
            {
                ResolutionWidth = Screen.currentResolution.width,
                ResolutionHeight = Screen.currentResolution.height,
                IsFullScreen = Screen.fullScreen,
                IsVsync = QualitySettings.vSyncCount == 1,
            } );
            
            storage.Audio.SetData( new AudioData()
            {
                MasterVolume = 100,
                DialogueVolume = 100,
                MusicVolume = 100,
                SFXVolume = 100,
                AmbientVolume = 100,
            } );
            
            storage.Controls.SetData( new ControlsData()
            {
                IsSprintToggle = false,
                IsCrouchToggle = true,
                MouseXSensitivity = 4f,
                MouseYSensitivity = 4f,
                ControllerXSensitivity = 120f,
                ControllerYSensitivity = 80f,
                IsControllerInvertXToggle = false,
                IsControllerInvertYToggle = false,
            } );
        }

        private bool AddDataForOldPlayers( GeneralStorage storage )
        {
            var isSupported = false;

            return isSupported;
        }
        
        private bool PurgeOldKeys( GeneralStorage storage )
        {
            var isPurged = false;

            return isPurged;
        }
    }
}