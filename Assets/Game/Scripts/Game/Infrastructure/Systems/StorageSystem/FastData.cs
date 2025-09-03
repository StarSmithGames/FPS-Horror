using PuzzlescapeGames.Storage;
using PuzzlescapeGames.Storage.Data;

namespace Game.Systems.StorageSystem
{
    public class FastData
    {
        private const string IS_FIRST_TIME = "is_first_time";
        private const string PREFERENCES_PARAMS = "preferences_params";
        
        public bool IsFirstTime
        {
            get => InputOutput.DeserializePlayerPrefs( IS_FIRST_TIME, true );
            set => InputOutput.SerializePlayerPrefs( IS_FIRST_TIME, value );
        }
    }
}