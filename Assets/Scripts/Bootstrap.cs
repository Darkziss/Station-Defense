using UnityEngine;
using StationDefense.InputSystem;
using StationDefense.UI;
using Pooling;
using PrimeTween;

namespace StationDefense
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;

        [SerializeField] private int _tweenCapacity = 5;

        [SerializeField] private InputHandler _inputHandler;

        [SerializeField] private MainPresenter _mainPresenter;

        [SerializeField] private DeathHandler _deathHandler;
        [SerializeField] private ScoreTracker _scoreTracker;

        [SerializeField] private BubbleTextSpawner _bubbleTextSpawner;

        [SerializeField] private CannonSelector _cannonSelector;
        [SerializeField] private Base _base;

        [SerializeField] private Cannon[] _cannons;
        [SerializeField] private Transform[] _cannonBases;

        private void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;

            PoolStorage.Init();

            PrimeTweenConfig.SetTweensCapacity(_tweenCapacity);

            _inputHandler.Init();

            _mainPresenter.Init();

            _deathHandler.Init();

            _bubbleTextSpawner.Init();

            //_scoreTracker.Init();

            _cannonSelector.Init(_cannons, _cannonBases);
            _base.Init();
        }
    }
}