using System;

namespace Game.Core.World.JournalSystem
{
    public sealed class ObjectiveMetadata
    {
        public event Action OnChanged;

        public string UID => Objective.UID;
        
        public QuestObjective Objective { get; }
        public ObjectiveState State { get; private set; }
        
        public ObjectiveMetadata( QuestObjective objective )
        {
            Objective = objective ?? throw new ArgumentNullException( nameof(objective) );
        }

        public void Dispose()
        {
            
        }

        public void SetState( ObjectiveState state )
        {
            if ( State != state )
            {
                State = state;
                OnChanged?.Invoke();
            }
        }
    }
}