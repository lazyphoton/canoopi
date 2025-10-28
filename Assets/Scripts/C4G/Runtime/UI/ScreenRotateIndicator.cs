using DG.Tweening;
using GameCore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c4g
{
    public class ScreenRotateIndicator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tabletIconObject;

        [SerializeField]
        private GameObject _mainPanelObject;

        private Sequence _sequence;

        private bool _visible;

        private void Start()
        {
            var rt = _tabletIconObject.GetComponent<RectTransform>();
            var img = _tabletIconObject.GetComponent<Image>();

            _sequence = DOTween.Sequence();

            _sequence.AppendCallback(() =>
            {
                rt.eulerAngles = Vector3.zero;
                img.color = new Color(1f, 1f, 1f, 0f);
            });
            _sequence.Append(img.DOFade(1f, 0.65f));
            _sequence.Append(rt.DORotate(new Vector3(0f, 0f, -90f), 0.45f).SetEase(Ease.InOutBack));
            _sequence.AppendInterval(1.2f);
            _sequence.Append(img.DOFade(0f, 0.5f));

            _sequence.SetLoops(-1, LoopType.Restart);
            _sequence.Play();

            SetVisibility(false);
        }

        private void Update()
        {
            SetVisibility(Screen.width < Screen.height);
        }

        private void SetVisibility(bool visible)
        {
            _visible = visible;

            _mainPanelObject.SetActive(_visible);
        }

        private void OnDestroy()
        {
            _sequence.Kill();
        }
    }
}