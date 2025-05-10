using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(BaseEnemyMover))]
    public class BaseEnemy : Enemy
    {
        [SerializeField] private BaseEnemyMover _baseEnemyMover;

        [SerializeField] private float _shootDelay;

        public override string EnemyName => nameof(BaseEnemy);

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_baseEnemyMover == null)
                _baseEnemyMover = GetComponent<BaseEnemyMover>();
        }

        public override void Init(ColorTeam team)
        {
            base.Init(team);

            _baseEnemyMover.Init(Team, _shootDelay);

            _baseEnemyMover.StartMoving();
        }

        protected override void StopAction()
        {
            base.StopAction();

            _baseEnemyMover.StopMoving();
            _baseEnemyMover.StopAttack();
        }
    }
}