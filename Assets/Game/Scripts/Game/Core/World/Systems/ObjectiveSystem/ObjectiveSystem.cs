using System;

namespace Game.Core.World.ObjectiveSystem
{
    public sealed class ObjectiveSystem
    {
        public event Action OnObjectiveChanged;

        public void SetCurrentObjective( string objectiveUID )
        {
            
            OnObjectiveChanged?.Invoke();
        }
        
        public string GetCurrentObjective()
        {
            return "";
        }
    }
}