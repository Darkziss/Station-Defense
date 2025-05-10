using UnityEngine;
using Pooling;

namespace StationDefense
{
    public class BaseShooter : MonoBehaviour
    {
        [SerializeField] private Transform[] _bulletPoints;
        [SerializeField] private Vector3[] _directions;

        [SerializeField] private Ball _ballPrefab;

        public void Shoot(ColorTeam team)
        {
            for (int i = 0; i < _bulletPoints.Length; i++)
            {
                Ball ball = PoolStorage.GetFromPool(_ballPrefab.BallName, _ballPrefab,
                    _bulletPoints[i].position, Quaternion.identity);

                ball.Init(team, _directions[i]);
            }
        }
    }
}