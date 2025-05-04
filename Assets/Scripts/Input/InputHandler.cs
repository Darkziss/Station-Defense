using UnityEngine;
using UnityEngine.InputSystem;

namespace StationDefense.InputSystem
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actions;

        public static InputAction SelectCannonAction { get; private set; }
        public static InputAction ResetCannonAction { get; private set; }

        public static InputAction LookAction { get; private set; }
        public static InputAction ShootAction { get; private set; }
        public static InputAction PowerfulShootAction { get; private set; }

        private const string actionMapName = "Main";

        private const string selectCannonActionName = "SelectCannon";
        private const string resetCannonActionName = "ResetCannon";

        private const string lookActionName = "Look";
        private const string shootActionName = "Shoot";
        private const string powerfulShootActionName = "PowerfulShoot";

        public void Init()
        {
            InputActionMap actionMap = _actions.FindActionMap(actionMapName);

            SelectCannonAction = actionMap.FindAction(selectCannonActionName);
            ResetCannonAction = actionMap.FindAction(resetCannonActionName);

            LookAction = actionMap.FindAction(lookActionName);
            ShootAction = actionMap.FindAction(shootActionName);
            PowerfulShootAction = actionMap.FindAction(powerfulShootActionName);
        }
    }
}