using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(Mover))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Mover _mover;

        [SerializeField] private int _barrierLayer;
        [SerializeField] private int _enemyLayer;

        private void OnValidate()
        {
            if (_mover == null)
                _mover = GetComponent<Mover>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            int collisionLayer = collision.gameObject.layer;

            if (collisionLayer == _barrierLayer || collisionLayer == _enemyLayer)
                Destroy(gameObject);
        }
    }
}