using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(TargetMover))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private TargetMover _mover;

        [SerializeField] private int _ballLayer;

        private const float fadeDuration = 0.5f;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_mover == null)
                _mover = GetComponent<TargetMover>();
        }

        private void Start()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;

            _spriteRenderer.color = color;
            
            Activate();
        }

        public void Activate()
        {
            _mover.StartMoving();
        }

        public void Deactivate()
        {
            _mover.StopMoving();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"Collision. Layer: {collision.gameObject.layer}");
            
            if (collision.gameObject.layer == _ballLayer)
            {
                Deactivate();

                Tween.Alpha(_spriteRenderer, 0f, fadeDuration)
                    .OnComplete(() => gameObject.SetActive(false));
            }
        }
    }
}