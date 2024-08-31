using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.VirtualCamera
{
    public class MainVirtualCameraController : MonoBehaviour
    {
        [SerializeField] private float zoomOutValue;
        [SerializeField] private float zoomingDuration;
        [SerializeField] private float zoomResetDuration;
        
        private CinemachineVirtualCamera _virtualCamera;
        private bool _isZoomedOut = false;
        private float _basicOrthoSize;

        private Tween _tween;
        private Sequence _resetTween;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();

            _basicOrthoSize = _virtualCamera.m_Lens.OrthographicSize;
        }

        public void ZoomOutAndResetWithDelay()
        {
            ResetTweens();
            _resetTween = DOTween.Sequence();
            
            var from = _virtualCamera.m_Lens.OrthographicSize;
            if (Mathf.Approximately(from, _basicOrthoSize))
            {
                _isZoomedOut = false;
            }
            
            var to = _isZoomedOut ? _basicOrthoSize : _basicOrthoSize + zoomOutValue;
            
            _tween = DOVirtual.Float(from, to, zoomingDuration, v => _virtualCamera.m_Lens.OrthographicSize = v);
            
            if (!_isZoomedOut)
            {
                _resetTween.Append(DOVirtual.Float(to, _basicOrthoSize, zoomingDuration, v => _virtualCamera.m_Lens.OrthographicSize = v)
                    .SetDelay(zoomingDuration + zoomResetDuration));
                _resetTween.Append(DOVirtual.DelayedCall(0f, () => _isZoomedOut = false));
            }
            
            _isZoomedOut = !_isZoomedOut;
        }

        public void ResetZoom()
        {
            ResetTweens();
            var from = _virtualCamera.m_Lens.OrthographicSize;
            _tween = DOVirtual.Float(from, _basicOrthoSize, zoomingDuration, v => _virtualCamera.m_Lens.OrthographicSize = v);
        }

        private void ResetTweens()
        {
            _tween.Kill();
            _resetTween.Kill();
        }

        private void OnDisable()
        {
            ResetTweens();
        }
    }
}
