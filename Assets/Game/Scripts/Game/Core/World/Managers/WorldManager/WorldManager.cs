using Game.Core.Player;
using Game.StoryFlow;

namespace Game.Core.World.WorldManager
{
    public sealed class WorldManager
    {
        public LevelObject Level { get; private set; }
        public PlayerObject Player { get; private set; }
        
        public WorldManager()
        {
            
        }

        public void SetLevel( LevelObject level )
        {
            Level = level;
        }

        public void SetPlayer( PlayerObject player )
        {
            Player = player;
        }
    }
}