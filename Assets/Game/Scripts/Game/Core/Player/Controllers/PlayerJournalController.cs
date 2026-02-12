using Game.Core.World.JournalSystem;

namespace Game.Core.Player
{
    public sealed class PlayerJournalController
    {
        public Journal Journal { get; }

        public PlayerJournalController( GameConfig gameConfig )
        {
            Journal = new( gameConfig.Journal );
        }

        public ObjectiveMetadata GetCurrentObjective()
        {
            return null;
        }
    }
}