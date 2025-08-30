using System;

namespace Game.SceneSystem
{
    public sealed class SceneNoLoadedException : Exception
    {
        public SceneNoLoadedException( string message ) : base( message )
        {
            
        }
    }
}