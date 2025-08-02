using Game.Core.Player;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using System;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public sealed class GameplayPipeline : IInitializable, IDisposable
    {
        public void Initialize()
        {
            CursorManager.Disable();
            InputManager.Initialize();
            
            GameObject.FindAnyObjectByType< PlayerObject >().Controller.Initialize();
        }

        public void Dispose()
        {
            InputManager.Dispose();
        }
    }
}