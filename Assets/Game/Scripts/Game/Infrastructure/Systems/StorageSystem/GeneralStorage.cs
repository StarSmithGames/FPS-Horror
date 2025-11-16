using PuzzlescapeGames.Storage;
using PuzzlescapeGames.Storage.Data;

namespace Game.Systems.StorageSystem
{
    public sealed class GeneralStorage : Storage
    {
        public StorageData< GameplayData > Gameplay { get; private set; }
        public StorageData< AudioData > Audio { get; private set; }
        public StorageData< GraphicsData > Graphics { get; private set; }
        public StorageData< ControlsData > Controls { get; private set; }
        
        public override void Purge()
        {
            Gameplay = new( Database, "gameplay" );
            Audio = new( Database, "audio" );
            Graphics = new( Database, "graphics" );
            Controls = new( Database, "controls" );
        }
    }
}