using PuzzlescapeGames.Storage;
using PuzzlescapeGames.Storage.Data;

namespace Game.Systems.StorageSystem
{
    public sealed class GeneralStorage : Storage
    {
        public StorageData< AudioData > Audio { get; private set; }
        public StorageData< GraphicsData > Graphics { get; private set; }
        public StorageData< ControlsData > Controls { get; private set; }
        
        public override void Purge()
        {
            Audio = new( Database, "audio" );
            Graphics = new( Database, "graphics" );
            Controls = new( Database, "controls" );
        }
    }
}