using UnityEngine;

namespace StationDefense
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Transform _firePointTransform;

        [SerializeField] private GameObject ballPrefab;

        private const int mouseButtonKey = 0;

        private void OnValidate()
        {
            _firePointTransform = GetComponentInChildren<Transform>();
        }

        private void Update()
        {
            bool mouseInput = Input.GetMouseButtonDown(mouseButtonKey);

            if (mouseInput)
            {
                Instantiate(ballPrefab, _firePointTransform.position, Quaternion.identity);
            }
        }
    }
}