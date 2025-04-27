using UnityEngine;
using PrimeTween;

namespace StationDefense
{
    public class CannonSelector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        [SerializeField] private Cannon[] _cannons = new Cannon[4];
        [SerializeField] private Transform[] _cannonBases = new Transform[4];

        [SerializeField] private KeyCode[] _cannonKeyCodes = new KeyCode[4];
        [SerializeField] private KeyCode _resetKeyCode = KeyCode.Space;

        private int _selectedIndex = defaultSelectedIndex;
        private Cannon _selectedCannon;

        private readonly TweenSettings _cameraTweenSettings = new(0.5f, Ease.OutCubic);

        private bool HaveSelectedCannon => _selectedCannon != null;

        private const int defaultSelectedIndex = -1;

        private const int shootMouseCode = 0;
        private const int inverseMouseCode = 1;

        public void ResetAll()
        {
            if (HaveSelectedCannon)
                _selectedCannon.Deactivate();
            
            _selectedIndex = defaultSelectedIndex;
            _selectedCannon = null;

            MoveCameraToPosition(Vector2.zero, false);
        }

        private void Update()
        {
            bool switchInput = CheckForInput(out int index);
            bool isSameIndex = index == _selectedIndex;

            if (switchInput && !isSameIndex)
            {
                if (HaveSelectedCannon)
                    _selectedCannon.Deactivate();
                
                _selectedIndex = index;
                _selectedCannon = _cannons[index];

                MoveCameraToPosition(_cannonBases[index].position, true);
                _selectedCannon.Activate();

                return;
            }

            if (!HaveSelectedCannon)
                return;

            bool shootInput = Input.GetMouseButtonDown(shootMouseCode);
            bool inverseInput = Input.GetMouseButtonDown(inverseMouseCode);
            bool resetInput = Input.GetKeyDown(_resetKeyCode);

            if (shootInput)
            {
                ShootFromCannon();
            }
            else if (resetInput)
            {
                _selectedCannon.Deactivate();

                _selectedIndex = defaultSelectedIndex;
                _selectedCannon = null;

                MoveCameraToPosition(Vector2.zero, true);
            }
            
            if (inverseInput)
            {
                _selectedCannon.InverseAngle();
            }
        }

        private void ShootFromCannon()
        {
            _selectedCannon.Shoot();
        }

        private bool CheckForInput(out int index)
        {
            for (int i = 0; i < _cannons.Length; i++)
            {
                bool input = Input.GetKeyDown(_cannonKeyCodes[i]);
                
                if (input)
                {
                    index = i;
                    return true;
                }
            }

            index = defaultSelectedIndex;
            return false;
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