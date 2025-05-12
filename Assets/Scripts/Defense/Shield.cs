using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Shield : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private ColorTeam _defenseTeam = ColorTeam.Red;

        private void OnValidate()
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.color = TeamColorStorage.GetByTeam(_defenseTeam);
        }

        public void ChangeDefenseTeam(ColorTeam team)
        {
            _defenseTeam = team;

            _spriteRenderer.color = TeamColorStorage.GetByTeam(team);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyBullet bullet = collision.gameObject.GetComponent<EnemyBullet>();

            if (bullet.Team == _defenseTeam)
                bullet.Disable();
        }
    }
}