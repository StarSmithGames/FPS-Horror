using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class UIGamepadTip : MonoBehaviour
    {
        [ SerializeField ] private GameObject _xbox;
        [ SerializeField ] private GameObject _ps5;

        public void SetType( int type )
        {
            _xbox.SetActive( type == 0 );
            _ps5.SetActive( type == 1 );
            
            gameObject.SetActive( type >= 0 );
        }
    }
}