using DG.Tweening;
using PuzzlescapeGames.IoC;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Core.World.IndicatorManager
{
    public sealed class InteractionIndicator : ZenjectMonoPoolable
    {
        [ SerializeField ] private Canvas _canvas;
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private Image _center;

        private Tween _tween;
        
        public override void OnSpawned( IMemoryPool pool )
        {
            base.OnSpawned( pool );
        }

        public override void OnDespawned()
        {
            base.OnDespawned();
        }
    }
}