using DG.Tweening;
using PuzzlescapeGames.IoC;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Core.World.PointerSystem
{
    public sealed class InteractionPointer : ZenjectMonoPoolable
    {
        [ SerializeField ] private Canvas _canvas;
        [ SerializeField ] private CanvasGroup _canvasGroup;
        [ SerializeField ] private Image _center;

        public bool IsShowing { get; private set; }
        
        private Tween _tween;
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
            IsShowing = true;
        }
        
        public void Hide()
        {
            _canvasGroup.alpha = 0;
            IsShowing = false;
            
            DespawnIt();
        }
        
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