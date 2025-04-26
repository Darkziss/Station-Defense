using UnityEngine;

namespace StationDefense
{
    [RequireComponent(typeof(Rotator))]
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Rotator _rotator;

        [SerializeField] private Transform _transform;
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private Mover ballPrefab;

        private const int mouseButtonKey = 0;

        private void OnValidate()
        {
            if (_rotator == null) _rotator = GetComponent<Rotator>();

            if (_transform == null) _transform = transform;
        }

        private void Update()
        {
            bool mouseInput = Input.GetMouseButtonDown(mouseButtonKey);

            if (mouseInput)
            {
                _rotator.StopRotating();
                
                Mover ballMover = Instantiate(ballPrefab, _firePointTransform.position, Quaternion.identity);

                ballMover.MoveDirection = _transform.up;
                ballMover.StartMoving();
            }
        }
    }
}