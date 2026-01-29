using UnityEngine;

namespace Game.Core.Entity
{
    public sealed class ComputerObject : PuzzleObject
    {
        [ SerializeField ] private ComputerCanvas _computerCanvas;
        
        public void EnableComputer( bool trigger ) => _computerCanvas.gameObject.SetActive( trigger );
    }
}