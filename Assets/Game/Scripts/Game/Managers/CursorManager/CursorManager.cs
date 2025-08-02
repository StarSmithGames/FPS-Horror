using UnityEngine;

namespace Game.Managers.CursorManager
{
    public static class CursorManager
    {
        public static bool IsVisible { get; private set; }
        
        public static void Enable()
        {
            IsVisible = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public static void Disable()
        {
            IsVisible = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}