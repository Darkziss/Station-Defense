using System;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    public class TurnCoordinator : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        [SerializeField] private List<Cannon> _cannons;

        [SerializeField] private bool _isTurnStarted = false;

        [SerializeField, Min(minTurn)] private int _currentTurn = minTurn;
        [SerializeField, Min(minCannonIndex)] private int _currentCannonIndex = minCannonIndex;

        private Cannon _currentCannon;

        private readonly TweenSettings _cameraTweenSettings = new(0.5f, Ease.OutCubic);

        private bool IsLastCannon => _currentCannonIndex == _cannons.Count - 1;

        private const int minTurn = -1;
        private const int minCannonIndex = -1;

        private const int mouseButtonKey = 0;

        private void OnValidate()
        {
            _isTurnStarted = false;
            
            _currentTurn = minTurn;
            _currentCannonIndex = minCannonIndex;
        }

        private void Update()
        {
            if (!_isTurnStarted)
                return;

            bool mouseInput = Input.GetMouseButtonDown(mouseButtonKey);

            if (!mouseInput)
                return;

            _currentCannon.Shoot();

            if (!IsLastCannon)
                NextCannon();
            else
                EndTurn();
        }

        public void StartTurn()
        {
            if (_isTurnStarted)
                throw new InvalidOperationException(nameof(_isTurnStarted));

            _isTurnStarted = true;

            _currentTurn++;

            NextCannon();
        }

        private void EndTurn()
        {
            _isTurnStarted = false;

            _currentCannonIndex = minCannonIndex;

            float cameraZPosition = _camera.transform.position.z;
            Vector3 cameraEndPosition = new(0f, 0f, cameraZPosition);

            Tween.Position(_camera.transform, cameraEndPosition, _cameraTweenSettings);

            Debug.Log($"Ended Turn: {_currentTurn}");
        }

        private void NextCannon()
        {
            _currentCannonIndex++;

            _currentCannon = _cannons[_currentCannonIndex];
            _currentCannon.Activate();

            Vector3 cannonPosition = _currentCannon.transform.position;
            float cameraZPosition = _camera.transform.position.z;

            Vector3 cameraEndPosition = new(cannonPosition.x, cannonPosition.y, cameraZPosition);

            Tween.Position(_camera.transform, cameraEndPosition, _cameraTweenSettings);
        }
    }
}