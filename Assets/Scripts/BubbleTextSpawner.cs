using System;
using UnityEngine;
using Pooling;
using UnityRandom = UnityEngine.Random;

namespace StationDefense
{
    public class BubbleTextSpawner : MonoBehaviour
    {
        [SerializeField] private BubbleText _bubbleTextPrefab;

        [SerializeField] private Material _outlineBlackMaterial;

        [SerializeField] private Material _outlineRedMaterial;
        [SerializeField] private Material _outlineYellowMaterial;
        [SerializeField] private Material _outlineGreenMaterial;
        [SerializeField] private Material _outlineBlueMaterial;

        private const float maxXOffset = 1f;

        public void Init()
        {
            Enemy.EnemyHit += SpawnDamageTextAtPosition;
        }

        private void SpawnDamageTextAtPosition(Vector2 position, int damage, ColorTeam team)
        {
            float xOffset = UnityRandom.Range(-maxXOffset, maxXOffset);
            position.x += xOffset;
            
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
                ColorTeam.None => _outlineBlackMaterial,
                _ => throw new ArgumentOutOfRangeException(nameof(team))
            };
        }
    }
}