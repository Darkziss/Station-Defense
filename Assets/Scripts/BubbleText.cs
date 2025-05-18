using System.Collections;
using UnityEngine;
using Pooling;
using TMPro;

namespace StationDefense
{
    public class BubbleText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private readonly WaitForSeconds _disableDelay = new(DisableDelay);

        private const float DisableDelay = 0.8f;

        private void OnValidate()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();
        }

        public void SetupAndDisableAfterDelay(string text, Color32 color, Material material)
        {
            _text.text = text;
            _text.color = color;
            _text.fontMaterial = material;

            StartCoroutine(DisableWithDelay());
        }

        private IEnumerator DisableWithDelay()
        {
            yield return _disableDelay;

            Disable();
        }

        private void Disable()
        {
            PoolStorage.PutToPool(nameof(BubbleText), this);
        }
    }
}