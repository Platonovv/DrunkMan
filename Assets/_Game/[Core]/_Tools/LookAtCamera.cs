using UnityEngine;

namespace _Tools
{
    public class LookAtCamera : MonoBehaviour
    {
        private Vector3 _rotationShift;
        private Transform _thisTransform;
        private Transform _cameraTransform;

        private void Awake()
        {
            _thisTransform = transform;
            _cameraTransform = Helper.Camera.transform;
        }

        private void LateUpdate()
        {
            _thisTransform.rotation = _cameraTransform.rotation;
        }
    }
}