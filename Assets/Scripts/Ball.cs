using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(Mover))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Mover _mover;

        [SerializeField] private int barrierLayer;

        private void OnValidate()
        {
            if (_mover == null) _mover = GetComponent<Mover>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            int collisionLayer = collision.gameObject.layer;

            if (collisionLayer == barrierLayer)
                Destroy(gameObject);
        }
    }
}