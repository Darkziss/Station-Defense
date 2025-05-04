using UnityEngine;
using UnityEngine.InputSystem;

namespace StationDefense.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actions;

        public static InputAction LookAction { get; private set; }
        public static InputAction ShootAction { get; private set; }

        private const string actionMapName = "Main";

        private const string lookActionName = "Look";
        private const string shootActionName = "Shoot";

        public void Init()
        {
            InputActionMap actionMap = _actions.FindActionMap(actionMapName);

            LookAction = actionMap.FindAction(lookActionName);
            ShootAction = actionMap.FindAction(shootActionName);
        }
    }
}