using UnityEngine;
using DG.Tweening;

namespace c4g
{
    public class UIBounce : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _target = new Vector3(0, 100f, 0);

        [SerializeField]
        private float _duration = 0.5f;

        private Tween _tween;

        private void Start()
        {
            var rt = GetComponent<RectTransform>();
            _tween = rt.DOLocalMove(_target, _duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuad);

        }

        private void OnDestroy()
        {
            _tween.Kill();
        }
    }
}