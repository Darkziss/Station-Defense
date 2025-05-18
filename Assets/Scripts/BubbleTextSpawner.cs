using System;
using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class BubbleTextSpawner : MonoBehaviour
    {
        [SerializeField] private BubbleText _bubbleTextPrefab;

        [SerializeField] private Material _outlineRedMaterial;
        [SerializeField] private Material _outlineYellowMaterial;
        [SerializeField] private Material _outlineGreenMaterial;
        [SerializeField] private Material _outlineBlueMaterial;

        public void Init()
        {
            Enemy.EnemyHit += SpawnDamageTextAtPosition;
        }

        private void SpawnDamageTextAtPosition(Vector2 position, int damage, ColorTeam team)
        {
            BubbleText instance = PoolStorage.GetFromPool(nameof(BubbleText), _bubbleTextPrefab, position,
                Quaternion.identity);

            string damageText = damage.ToString();
            Color32 color = TeamColorStorage.GetByTeam(team);
            Material material = GetMaterialByTeam(team);

            instance.SetupAndDisableAfterDelay(damageText, color, material);
        }

        private Material GetMaterialByTeam(ColorTeam team)
        {
            return team switch
            {
                ColorTeam.Red => _outlineRedMaterial,
                ColorTeam.Yellow => _outlineYellowMaterial,
                ColorTeam.Green => _outlineGreenMaterial,
                ColorTeam.Blue => _outlineBlueMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(team))
            };
        }
    }
}