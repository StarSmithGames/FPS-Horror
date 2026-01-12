using Game.Core.Player;

namespace Game.Core.World.EntityManager
{
    public sealed class EntityManager
    {
        public PlayerObject Player { get; private set; }
        
        public EntityManager()
        {
            
        }

        public void SetPlayer( PlayerObject player )
        {
            Player = player;
        }
    }
}