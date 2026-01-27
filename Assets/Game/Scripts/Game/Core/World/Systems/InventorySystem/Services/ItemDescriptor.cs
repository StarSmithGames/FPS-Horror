using PuzzlescapeGames.Localization;
using System;

namespace Game.Core.World.InventorySystem
{
    public sealed class ItemDescriptor
    {
        private readonly ILocalizationSystem _localizationSystem;
        
        public ItemDescriptor( ILocalizationSystem localizationSystem )
        {
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        public string GetName( ItemConfig config )
        {
            return config.NameId;
        }

        public string GetDescription( ItemConfig config )
        {
            return config.DescriptionId;
        }

        public string GetType( ItemConfig config )
        {
            return "Common";
        }
    }
}