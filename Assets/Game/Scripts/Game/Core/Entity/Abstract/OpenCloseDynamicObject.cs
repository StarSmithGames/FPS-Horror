namespace Game.Core.Entity
{
    public abstract class OpenCloseDynamicObject : DynamicObject
    {
        public bool IsOpen { get; protected set; }
        
        public abstract void Open();
        public abstract void Close();
        
        public void Toggle()
        {
            if ( IsOpen )
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}