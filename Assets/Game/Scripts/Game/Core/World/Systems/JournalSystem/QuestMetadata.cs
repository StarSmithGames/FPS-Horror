using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.World.JournalSystem
{
    public sealed class QuestMetadata
    {
        public event Action OnChanged;
        
        public List< ObjectiveMetadata > Objectives { get; } = new();
        
        public Quest Quest { get; }

        public QuestMetadata( Quest quest )
        {
            Quest = quest ?? throw new ArgumentNullException( nameof(quest) );

            for ( int i = 0; i < quest.Objectives.Count; i++ )
            {
                ObjectiveMetadata metadata = new( quest.Objectives[ i ] );
                metadata.OnChanged += MetadataChangedHandler;
                Objectives.Add( metadata );
            }
        }

        public void Dispose()
        {
            for ( var i = 0; i < Objectives.Count; i++ )
            {
                var objective = Objectives[ i ];
                objective.OnChanged -= MetadataChangedHandler;
                objective.Dispose();
            }
        }
        
        public bool Contains( string UID ) => Objectives.FirstOrDefault( ( x ) => string.Equals( x.UID, UID, StringComparison.InvariantCultureIgnoreCase ) ) != null;
        
        private void MetadataChangedHandler() => OnChanged?.Invoke();
    }
}