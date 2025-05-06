using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(TargetMover))]
    public class FastEnemy : Enemy
    {
        [SerializeField] private TargetMover _targetMover;

        public override string EnemyName => nameof(FastEnemy);

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_targetMover == null)
                _targetMover = GetComponent<TargetMover>();
        }

        public override void Init(ColorTeam team)
        {
            base.Init(team);

            _targetMover.StartMoving();
        }

        protected override void StopAction()
        {
            base.StopAction();

            _targetMover.StopMoving();
        }
    }
}