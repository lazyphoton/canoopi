using DG.Tweening;
using UnityEngine;

namespace c4g
{
    public class SeedPlantingTarget : MonoBehaviour
    {
        [SerializeField]
        private SeedSortingTag _seedSortingTag;

        [SerializeField]
        private GameObject _iconPanelObject;

        [SerializeField]
        private GameObject _colliderObject;

        [SerializeField]
        private GameObject _plantEffectPrefab;

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
        }

        public void OnCorrectSeed()
        {
            Instantiate(_plantEffectPrefab, transform.position, Quaternion.identity);

            _iconPanelObject.SetActive(false);
            _colliderObject.SetActive(false);
        }

        public void OnIncorrectSeed()
        {
            KillTween();

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOShakeScale(0.6f, new Vector3(0.3f, 0.7f, 0.3f), 25));
            _sequence.Join(transform.DOShakeRotation(0.5f, new Vector3(0f, 20f, 0f), 15));
            _sequence.Play();
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