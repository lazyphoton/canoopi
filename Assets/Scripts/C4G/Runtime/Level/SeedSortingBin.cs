using UnityEngine;
using DG.Tweening;

namespace c4g
{
    public enum SeedSortingTag
    {
        Pile,
        Grass,
        Millet,
        Sorghum,
        Dirt,
        None
    }

    public class SeedSortingBin : MonoBehaviour
    {
        [SerializeField]
        private SeedSortingTag _seedSortingTag;

        [SerializeField]
        private float _startingFullness = 0f;

        [SerializeField]
        private float _fullPosition = 1f;

        [SerializeField]
        private float _emptyPosition = 0f;

        [SerializeField]
        private GameObject _fillerObject;

        public SeedSortingTag SeedSortingTag => _seedSortingTag;

        private Sequence _sequence;

        private Vector3 _startingPosition;
        private Vector3 _startingScale;
        private Quaternion _startingRotation;

        private void Start()
        {
            _startingPosition = transform.position;
            _startingScale = transform.localScale;
            _startingRotation = transform.localRotation;

            SetFullness(_startingFullness);
        }

        public void SetFullness(float fullness)
        {
            if(_fillerObject == null)
            {
                return;
            }

            var fillPos = _fillerObject.transform.localPosition;
            fillPos.y = Mathf.Lerp(_emptyPosition, _fullPosition, fullness);

            _fillerObject.transform.localPosition = fillPos;
        }

        public void OnCorrectSeed()
        {
            KillTween();

            _sequence = transform.DOLocalJump(_startingPosition, 0.5f, 1, 0.3f);
            _sequence.Append(transform.DOLocalJump(_startingPosition, 0.25f, 1, 0.22f));
            _sequence.Append(transform.DOLocalJump(_startingPosition, 0.125f, 1, 0.16f));

            _sequence.Play();
        }

        public void OnIncorrectSeed()
        {
            KillTween();

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOShakeScale(0.6f, new Vector3(0.3f, 0.7f, 0.3f), 25));
            _sequence.Join(transform.DOShakeRotation(0.5f, new Vector3(0f, 20f, 0f), 15));
            _sequence.Play();
        }

        public void OnTakeSeed()
        {
            KillTween();

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOPunchScale(new Vector3(0.4f, -0.3f, 0.4f), 0.65f, 5, 0.05f));
            _sequence.Play();
        }

        private void OnDestroy()
        {
            KillTween();
        }

        private void KillTween()
        {
            _sequence.Kill();

            transform.position = _startingPosition;
            transform.localScale = _startingScale;
            transform.localRotation = _startingRotation;
        }
    }
}