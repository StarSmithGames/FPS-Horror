using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.World.JournalSystem
{
    public sealed class Journal
    {
        public event Action OnChanged;
        
        public List< QuestMetadata > Quests { get; private set; } = new();

        
        public Journal( JournalConfig config )
        {
            foreach ( var quest in config.Quests )
            {
                AddQuest( quest );
            }
        }
        
        public void Dispose()
        {
            for ( var i = 0; i < Quests.Count; i++ )
            {
                var quest = Quests[ i ];
                quest.OnChanged -= MetadataChangedHandler;
                quest.Dispose();
            }
        }

        public void Load()
        {
            
        }

        private void AddQuest( Quest quest )
        {
            QuestMetadata metadata = new( quest );
            metadata.OnChanged += MetadataChangedHandler;
            Quests.Add( metadata );
        }

        private void MetadataChangedHandler()
        {
            OnChanged?.Invoke();
        }
    }
}