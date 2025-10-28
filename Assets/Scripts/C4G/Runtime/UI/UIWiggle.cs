using DG.Tweening;
using UnityEngine;

namespace c4g
{
    public class UIWiggle : MonoBehaviour
    {
        private Sequence _sequence;

        private void Start()
        {
            var rt = GetComponent<RectTransform>();

            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(1f);
            
            _sequence.Append(rt.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), 0.9f, 6, 0.5f).SetEase(Ease.InSine));
            _sequence.Insert(1f, rt.DOShakeRotation(1.5f, 20f, 5, 50, true, ShakeRandomnessMode.Harmonic).SetEase(Ease.InSine));
            _sequence.AppendInterval(0.5f);

            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.Play();
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}