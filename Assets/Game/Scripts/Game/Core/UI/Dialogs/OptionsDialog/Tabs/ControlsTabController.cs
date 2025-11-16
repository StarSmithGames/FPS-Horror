using Cysharp.Threading.Tasks;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class ControlsTabController : TabController
    {
        private OptionKeyController _forward;
        private OptionKeyController _backward;
        private OptionKeyController _left;
        private OptionKeyController _right;
        private OptionKeyController _sprint;
        private OptionKeyController _crouch;
        private OptionKeyController _interact;
        private OptionKeyController _reloadWeapon;

        
        private readonly ControlsData _data;
        
        public ControlsTabController(
            TabsSettings settings,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
        ) : base( settings, localizationSystem )
        {
            _data = dataHolder.GeneralStorageData.Controls.Value;
        }

        public override void Save()
        {
            base.Save();
        }

        protected override async UniTask Load( Transform content, CancellationToken cancellationToken = default )
        {
            //LocalizationIds.ITEM_DOOR
            _forward = CreateKey( content, "Forward" );
            _backward = CreateKey( content, "Backward" );
            _left = CreateKey( content, "Left" );
            _right = CreateKey( content, "Right" );
            _sprint = CreateKey( content, "Sprint" );
            _crouch = CreateKey( content, "Crouch" );
            _interact = CreateKey( content, "Interact" );
            _reloadWeapon = CreateKey( content, "Reload weapon" );
        }
    }
}