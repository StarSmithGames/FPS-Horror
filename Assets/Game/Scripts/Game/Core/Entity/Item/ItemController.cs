namespace Game.Core.Entity
{
    public abstract class ItemController
    {
        public ItemObject View { get; }
        
        protected ItemController( ItemObject view )
        {
            View = view;
        }
    }
}