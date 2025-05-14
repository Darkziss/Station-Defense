using UnityEngine;

namespace StationDefense
{
    [CreateAssetMenu(fileName = "Group", menuName = "ScriptableObjects/EnemyGroup")]
    public class EnemyGroup : ScriptableObject
    {
        [SerializeField, Min(MinCount)] private int _baseEnemyCount;
        [SerializeField, Min(MinCount)] private int _fastEnemyCount;
        [SerializeField, Min(MinCount)] private int _bigEnemyCount;

        public int BaseEnemyCount => _baseEnemyCount;
        public int FastEnemyCount => _fastEnemyCount;
        public int BigEnemyCount => _bigEnemyCount;

        private const int MinCount = 0;
    }
}