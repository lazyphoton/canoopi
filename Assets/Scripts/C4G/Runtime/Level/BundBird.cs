using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace c4g
{
    public class BundBird : MonoBehaviour
    {
        private bool _scared = false;

        public bool Scared => _scared;

        private GameObject _player;

        private float _distanceTolerance;

        private List<Vector3> _bundLocations;

        private bool _initialized = false;

        private Vector3 _targetPosition;

        private bool _flying;

        public void Initialize(GameObject player, List<Vector3> bundLocations)
        {
            _player = player;
            _bundLocations = bundLocations;

            _distanceTolerance = UnityEngine.Random.Range(2f, 5f);

            _flying = true;

            transform.position = 20f * GetRandomOffset() + (Vector3.up * 10f);
            _targetPosition = GetTargetLocation();
            transform.LookAt(_targetPosition);

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(_targetPosition, 2f).SetEase(Ease.InOutSine));
            sequence.AppendCallback(() => 
            { 
                _flying = false; 
            });
            sequence.Play();

            _initialized = true;
        }

        private Vector3 GetTargetLocation()
        {
            // Get one of the farther bunds

            var fartherBunds = _bundLocations.OrderByDescending(x => (x - _player.transform.position).sqrMagnitude).Take(4).ToList();

            return fartherBunds[UnityEngine.Random.Range(0, fartherBunds.Count)] + GetRandomOffset();
        }

        private Vector3 GetRandomOffset()
        {
            var r = UnityEngine.Random.insideUnitCircle;
            return new Vector3(2f*r.x, 0f, 2f*r.y);
        }

        private void Update()
        {
            if (!_initialized)
            {
                return;
            }

            if (_flying)
            {
                return;
            }

            var distVector = transform.position - _player.transform.position;

            if (distVector.magnitude < _distanceTolerance)
            {
                if(UnityEngine.Random.Range(0f, 1f) < 0.5f)
                {
                    FlyScared(distVector);
                }
                else
                {
                    FlyToOtherBund();
                }
            }
        }

        private void FlyScared(Vector3 distVector)
        {
            _scared = true;
            _flying = true;

            _targetPosition = (40f * distVector.normalized) + (Vector3.up * 10f);
            transform.LookAt(_targetPosition);

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(_targetPosition, 2f).SetEase(Ease.InOutSine));
            sequence.Play();
        }

        private void FlyToOtherBund()
        {
            _flying = true;

            _targetPosition = GetTargetLocation();
            transform.LookAt(_targetPosition);

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOJump(_targetPosition, 4f, 1, 1.5f));
            sequence.AppendCallback(() =>
            {
                _flying = false;
            });
            sequence.Play();
        }
    }
}