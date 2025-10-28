using c4g;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class UIManager : MonoBehaviour
    {
        public event Action DialogStarted;
        public event Action DialogFinished;

        [Header("Main Canvas")]
        [SerializeField]
        private Canvas _canvas;

        [Header("Transitions")]
        [SerializeField]
        private GameObject _transitionCanvasPrefab;

        private Stack<AUIFrame> _uiFrames;

        private Transform _parentPanelTransform;

        private readonly string _transitionCanvasName = "PersistentTransitionCanvas";
        private GameObject _transitionBlockerPanel;
        private Transform _transitionParentPanel;
        private Material _sceneTransitionMaterial;

        private float _sceneTransitionDuration = 0.45f;
        private float _sceneTransitionWaitDuration = 0.15f;
        private Sequence _sceneTransitionSequence;


        private void Start()
        {
            _parentPanelTransform = _canvas.transform.Find("ParentPanel");

            _uiFrames = new Stack<AUIFrame>();

            var uiFrameDefinitions = Resources.LoadAll<UIFrameDefinition>("UIFrameDefinitions");

            if (_transitionCanvasPrefab == null) 
            {
                Log.Warning("No UI transition canvas prefab present.");
                return;
            }

            var transitionCanvasObj = GameObject.Find(_transitionCanvasName);

            if (transitionCanvasObj == null)
            {
                transitionCanvasObj = Instantiate(_transitionCanvasPrefab);
                transitionCanvasObj.name = _transitionCanvasName;
                DontDestroyOnLoad(transitionCanvasObj);

                _transitionParentPanel = transitionCanvasObj.transform.Find("ParentPanel");
                var img = _transitionParentPanel.GetComponent<Image>();
                _sceneTransitionMaterial = new Material(img.material);
                img.material = _sceneTransitionMaterial;
            }
            else
            {
                _transitionParentPanel = transitionCanvasObj.transform.Find("ParentPanel");
                var img = _transitionParentPanel.GetComponent<Image>();
                _sceneTransitionMaterial = img.material;
            }

            _transitionBlockerPanel = transitionCanvasObj.transform.Find("BlockerPanel").gameObject;
        }

        public void SetUI(UIFrameDefinition uiFrameDefinition, Dictionary<string, object> injectedInfo)
        {
            while (_uiFrames.Count > 0)
            {
                PopUI(injectedInfo);
            }

            PushUI(uiFrameDefinition, injectedInfo);
        }

        public void PushUI(UIFrameDefinition uiFrameDefinition)
        {
            PushUI(uiFrameDefinition, new Dictionary<string, object>());
        }

        public void PushUI(UIFrameDefinition uiFrameDefinition, Dictionary<string, object> injectedInfo)
        {
            if (uiFrameDefinition == null)
            {
                Log.Error("Null UI frame definition.");
                return;
            }

            if (uiFrameDefinition.UIFramePrefab == null)
            {
                Log.Error("Null UI frame prefab.");
                return;
            }

            var obj = Instantiate(uiFrameDefinition.UIFramePrefab, _parentPanelTransform);

            if (obj.TryGetComponent<AUIFrame>(out var frame))
            {
                _uiFrames.Push(frame);
                frame.OnPush(this, injectedInfo);
            }
            else
            {
                Destroy(obj);
                Log.Error("UI frame prefab does not have an ui frame component inheriting from AUIFrame.");
            }
        }

        public void PopUI(Dictionary<string, object> injectedInfo)
        {
            var poppedFrame = _uiFrames.Pop();
            poppedFrame.OnPop(injectedInfo);
            Destroy(poppedFrame.gameObject);

            if (_uiFrames.Count > 0)
            {
                _uiFrames.Peek().OnFallbackFocus(injectedInfo);
            }
        }

        public void DoSceneTransitionHide(Action onComplete = null)
        {
            if (_sceneTransitionSequence != null)
            {
                Log.Error("Attempting to do hide scene transition when scene transition sequence is already going.");
                return;
            }

            SetSceneTransitionFactor(0f);
            _transitionBlockerPanel.SetActive(true);

            _sceneTransitionSequence = DOTween.Sequence();

            _sceneTransitionSequence.Append(DOVirtual.Float(0f, 1f, _sceneTransitionDuration, (float val) => {
                SetSceneTransitionFactor(val);
            }));

            _sceneTransitionSequence.AppendInterval(_sceneTransitionWaitDuration);

            _sceneTransitionSequence.AppendCallback(() => {
                SetSceneTransitionFactor(1f);
                _transitionBlockerPanel.SetActive(false);
                onComplete?.Invoke();
            });

            _sceneTransitionSequence.OnKill(() => _sceneTransitionSequence = null);
            _sceneTransitionSequence.Play();
        }

        public void DoSceneTransitionShow(Action onComplete = null)
        {
            if (_sceneTransitionSequence != null)
            {
                Log.Error("Attempting to do show scene transition when scene transition sequence is already going.");
                return;
            }

            SetSceneTransitionFactor(1f);
            _transitionBlockerPanel.SetActive(true);

            _sceneTransitionSequence = DOTween.Sequence();

            _sceneTransitionSequence.AppendInterval(_sceneTransitionWaitDuration);

            _sceneTransitionSequence.Append(DOVirtual.Float(0f, 1f, _sceneTransitionDuration, (float val) => {
                SetSceneTransitionFactor(1f - val);
            }));

            _sceneTransitionSequence.AppendCallback(() => {
                SetSceneTransitionFactor(0f);
                _transitionBlockerPanel.SetActive(false);
                onComplete?.Invoke();
            });

            _sceneTransitionSequence.OnKill(() => _sceneTransitionSequence = null);
            _sceneTransitionSequence.Play();
        }

        private void SetSceneTransitionFactor(float factor)
        {
            //_transitionParentPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, factor);
            _sceneTransitionMaterial.SetFloat("_CutoffThreshold", 1 - factor);
        }

        public void SetDialogStarted()
        {
            DialogStarted?.Invoke();
        }

        public void SetDialogFinished()
        {
            DialogFinished?.Invoke();
        }
    }
}