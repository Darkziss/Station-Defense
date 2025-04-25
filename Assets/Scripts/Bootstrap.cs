using UnityEngine;

namespace StationDefense
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}