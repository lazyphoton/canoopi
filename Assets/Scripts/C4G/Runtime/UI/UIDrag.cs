using DG.Tweening;
using UnityEngine;

namespace c4g
{
    public class UIDrag : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _target = new Vector3(0, 100f, 0);

        [SerializeField]
        private float _duration = 0.5f;

        private Sequence _sequence;

        private void Start()
        {
            var rt = GetComponent<RectTransform>();

            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(0.5f);
            _sequence.Append(rt.DOLocalMove(_target, _duration).SetEase(Ease.InQuad));
            _sequence.AppendInterval(1f);
            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.Play();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}