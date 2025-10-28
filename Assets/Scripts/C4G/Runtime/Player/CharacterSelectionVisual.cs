using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace c4g
{
    public class CharacterSelectionVisual : MonoBehaviour
    {
        [SerializeField]
        private Transform _sphereParent;

        [SerializeField]
        private Transform _characterVisualParent;

        private int _numPlayerVisuals;

        public void Initialize(GameObject[] playerVisualPrefabs)
        {
            _numPlayerVisuals = playerVisualPrefabs.Length;

            for(int i = 0; i < _numPlayerVisuals; i++)
            {
                var obj = Instantiate(playerVisualPrefabs[i]);
                obj.transform.SetParent(_characterVisualParent);

                var angle = -((float)i / _numPlayerVisuals) * Mathf.PI * 2f;
                var posAngle = angle + (Mathf.PI / 2f);
                var rotAngle = angle;
                var sphereSize = 2.5f;

                var localPos = new Vector3(sphereSize * Mathf.Cos(posAngle), sphereSize * Mathf.Sin(posAngle), 0f);
                var localAngles = new Vector3(0f, 180f, -rotAngle * Mathf.Rad2Deg);

                obj.transform.localPosition = localPos;
                obj.transform.localEulerAngles = localAngles;
            }
        }

        public void MoveToIndex(int index, Action onComplete)
        {
            var angle = ((float)(index) / _numPlayerVisuals) * Mathf.PI * 2f;

            var targetEulers = new Vector3(0f, 0f, angle * Mathf.Rad2Deg);

            _sphereParent.transform.DOLocalRotate(targetEulers, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => { onComplete?.Invoke(); });
        }
    }
}