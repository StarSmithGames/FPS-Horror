using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIInventoryCell : MonoBehaviour
    {
        [ SerializeField ] private GameObject _opened;
        [ SerializeField ] private GameObject _locked;

        public void SetLock( bool trigger )
        {
            _opened.SetActive( !trigger );
            _locked.SetActive( trigger );
        }
    }
}