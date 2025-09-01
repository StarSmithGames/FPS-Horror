using System;
using UnityEngine;

namespace Game.Managers.GameManager
{
    public sealed class GameManager
    {
        public event Action OnGameStateChanged;

        public bool IsMenu => CurrentGameState == GameState.Menu;
        public bool IsGame => CurrentGameState == GameState.Game;

        public GameState CurrentGameState { get; private set; } = GameState.Empty;
        public GameState PreviousGameState { get; private set; } = GameState.Empty;

        public void SetState( GameState state )
        {
            if ( CurrentGameState != state )
            {
                PreviousGameState = CurrentGameState;
                CurrentGameState = state;

                OnGameStateChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning( $"[GameManager] Try to set state: {state}, but it's already setted." );
            }
        }
    }
}