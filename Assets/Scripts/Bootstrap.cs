using UnityEngine;
using StationDefense.UI;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;

        [SerializeField] private int _tweenCapacity = 5;

        [SerializeField] private MainPresenter _mainPresenter;

        [SerializeField] private DeathHandler _deathHandler;

        [SerializeField] private Base _base;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;

            PoolStorage.Init();

            PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);

            _mainPresenter.Init();

            _deathHandler.Init();

            _base.Init();
        }
    }
}