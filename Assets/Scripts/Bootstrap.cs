using PrimeTween;
using UnityEngine;

namespace StationDefense
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;

        [SerializeField] private int _tweenCapacity = 5;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;

            PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);
        }
    }
}