using UnityEngine;
using UnityEngine.InputSystem;

namespace StationDefense.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Cannon[] _cannons;

        [SerializeField] private InputActionAsset _actions;

        private InputAction _shootAction;

        private const string actionMapName = "Main";

        private const string shootActionName = "Shoot";

        public void Init()
        {
            InputActionMap actionMap = _actions.FindActionMap(actionMapName);

            _shootAction = actionMap.FindAction(shootActionName);

            _shootAction.Enable();

            foreach (Cannon cannon in _cannons)
            {
                _shootAction.performed += (_) => cannon.OnShootStarted();
                _shootAction.canceled += (_) => cannon.OnShootStopped();
            }
        }
    }
}