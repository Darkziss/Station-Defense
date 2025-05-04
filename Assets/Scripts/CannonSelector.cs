using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StationDefense.InputSystem;
using PrimeTween;
using System;

namespace StationDefense
{
    public class CannonSelector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        private Vector2Int _selectedKey;
        private Cannon _selectedCannon;

        private InputAction _selectCannonAction;
        private InputAction _resetCannonAction;

        private readonly Dictionary<Vector2Int, Cannon> _cannons = new(cannonCount);
        private readonly Dictionary<Vector2Int, Transform> _cannonBases = new(cannonCount);

        private readonly Vector2Int _defaultSelectedKey = Vector2Int.zero;

        private readonly TweenSettings _cameraTweenSettings = new(0.5f, Ease.OutCubic);

        private bool HaveSelectedCannon => _selectedCannon != null;

        private const int cannonCount = 4;

        public void Init(Cannon[] cannons, Transform[] cannonBases)
        {
            _selectedKey = _defaultSelectedKey;
            
            Span<Vector2Int> keys = stackalloc Vector2Int[cannonCount]
            {
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down
            };

            for (int i = 0; i < cannonCount; i++)
            {
                _cannons.Add(keys[i], cannons[i]);
                _cannonBases.Add(keys[i], cannonBases[i]);
            }

            _selectCannonAction = InputHandler.SelectCannonAction;
            _resetCannonAction = InputHandler.ResetCannonAction;

            _selectCannonAction.Enable();
            _resetCannonAction.Enable();

            _resetCannonAction.performed += (_) =>
            {
                if (HaveSelectedCannon)
                    ResetCannonSelection();
            };
        }

        private void Update()
        {
            if (_selectCannonAction.IsPressed())
            {
                Vector2 select = _selectCannonAction.ReadValue<Vector2>();
                Vector2Int key = InputToKey(select);

                if (key == _selectedKey)
                    return;

                if (HaveSelectedCannon)
                    _selectedCannon.Deactivate();

                _selectedKey = key;
                _selectedCannon = _cannons[key];

                _selectedCannon.Activate();

                MoveCameraToPosition(_cannonBases[key].position, true);
            }

            if (!HaveSelectedCannon)
                return;
        }

        public void ResetAll()
        {
            if (HaveSelectedCannon)
                _selectedCannon.Deactivate();

            _selectedKey = _defaultSelectedKey;
            _selectedCannon = null;

            MoveCameraToPosition(Vector2.zero, false);
        }

        private void ResetCannonSelection()
        {
            _selectedCannon.Deactivate();

            _selectedKey = _defaultSelectedKey;
            _selectedCannon = null;

            MoveCameraToPosition(Vector2.zero, true);
        }

        private Vector2Int InputToKey(Vector2 input)
        {
            if (input.x > 0f)
                return Vector2Int.right;
            else if (input.x < 0f)
                return Vector2Int.left;
            else if (input.y > 0f)
                return Vector2Int.up;
            else if (input.y < 0f)
                return Vector2Int.down;
            else
                return Vector2Int.zero;
        }

        private void MoveCameraToPosition(Vector2 position, bool animate)
        {
            float cameraZPosition = _camera.transform.position.z;
            Vector3 endPosition = new(position.x, position.y, cameraZPosition);

            if (animate)
                Tween.Position(_camera.transform, endPosition, _cameraTweenSettings);
            else
                _camera.transform.position = endPosition;
        }
    }
}