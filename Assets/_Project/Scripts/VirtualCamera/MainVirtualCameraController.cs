using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.VirtualCamera
{
    public class MainVirtualCameraController : MonoBehaviour
    {
        [SerializeField] private float zoomOutValue;
        [SerializeField] private float duration;
        
        private CinemachineVirtualCamera _virtualCamera;
        private bool _isZoomedOut = false;
        private float _basicOrthoSize;

        private Tween _tween;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();

            _basicOrthoSize = _virtualCamera.m_Lens.OrthographicSize;
        }

        public void ChangeZoom()
        {
            _tween.Kill();
            var from = _virtualCamera.m_Lens.OrthographicSize;
            var to = _isZoomedOut ? _basicOrthoSize : _basicOrthoSize + zoomOutValue;

            _tween = DOVirtual.Float(from, to, duration, v => _virtualCamera.m_Lens.OrthographicSize = v);
            _isZoomedOut = !_isZoomedOut;
        }

        private void OnDisable()
        {
            _tween.Kill();
        }
    }
}
