using UnityEngine;

namespace StationDefense.UI
{
    public class Panel : MonoBehaviour
    {
        public bool IsVisible => gameObject.activeInHierarchy;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}