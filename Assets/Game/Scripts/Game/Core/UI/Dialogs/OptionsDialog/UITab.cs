using System;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class UITab : MonoBehaviour
    {
        public event Action< UITab > OnButtonClicked;

        public void OnButtonClick()
        {
            OnButtonClicked?.Invoke( this );
        }
    }
}