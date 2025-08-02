using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.UI
{
    public sealed class UIInfoButton : MonoBehaviour
    {
        [ SerializeField ] private TMPro.TextMeshProUGUI _buttonName;
        [ SerializeField ] private Image _buttonProgress;
        [ SerializeField ] private TMPro.TextMeshProUGUI _buttonActionText;

        public void Set( string name, string actionText )
        {
            _buttonName.text = name;
            _buttonActionText.text = actionText;
        }

        public void SetFillAmount( float value )
        {
            _buttonProgress.fillAmount = value;
        }
    }
}