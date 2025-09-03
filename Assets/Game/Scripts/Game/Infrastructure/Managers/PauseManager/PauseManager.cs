using PuzzlescapeGames.Services;
using System;

namespace Game.Managers.PauseManager
{
    public sealed class PauseManager: Observer< IPauseable >
    {
        public event Action OnPauseChanged;

        public bool IsPause { get; private set; }
        
        public override void AddObserver( IPauseable observer, bool notify = true )
        {
            if ( IsPause )
            {
                observer.Pause();
            }
            else
            {
                observer.UnPause();
            }
			
            base.AddObserver( observer, notify );
        }
        
        public void Pause()
        {
            IsPause = true;

            PauseChanged();
            OnPauseChanged?.Invoke();
        }

        public void UnPause()
        {
            IsPause = false;

            PauseChanged();
            OnPauseChanged?.Invoke();
        }

        private void PauseChanged()
        {
            if ( IsPause )
            {
                for ( int i = 0; i < Observables.Count; i++ )
                {
                    Observables[i].Pause();
                }
            }
            else
            {
                for ( int i = 0; i < Observables.Count; i++ )
                {
                    Observables[i].UnPause();
                }
            }
        }
    }
}