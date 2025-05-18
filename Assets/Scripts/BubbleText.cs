using UnityEngine;
using Pooling;
using TMPro;

namespace StationDefense
{
    public class BubbleText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [SerializeField] private Material _outlineRedMaterial;
        [SerializeField] private Material _outlineYellowMaterial;
        [SerializeField] private Material _outlineGreenMaterial;
        [SerializeField] private Material _outlineBlueMaterial;

        //private const float OutlineColorFactor = 0.85f;

        private const float DisableDelay = 0.5f;

        private void OnValidate()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();
        }

        public void SetText(string text, ColorTeam team)
        {
            _text.text = text;

            Color32 color = TeamColorStorage.GetByTeam(team);
            //Color32 outlineColor = Color32.Lerp(color, Color.black, OutlineColorFactor);

            _text.color = color;
            _text.fontMaterial = GetMaterialByTeam(team);

            Invoke(nameof(Disable), DisableDelay);
        }

        private void Disable()
        {
            PoolStorage.PutToPool(nameof(BubbleText), this);
        }

        private Material GetMaterialByTeam(ColorTeam team)
        {
            return team switch
            {
                ColorTeam.Red => _outlineRedMaterial,
                ColorTeam.Yellow => _outlineYellowMaterial,
                ColorTeam.Green => _outlineGreenMaterial,
                ColorTeam.Blue => _outlineBlueMaterial,
                _ => _outlineRedMaterial
            };
        }
    }
}