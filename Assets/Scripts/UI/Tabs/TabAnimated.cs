using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Tabs
{
    public class TabAnimated : MonoBehaviour
    {
        [SerializeField] private RectTransform[] animatedObjects;
        [SerializeField] private Button backButton;
        [Inject] private TabsController _tabs;

        private const float MoveAmplitude = 30f;
        private const float RotateAmplitude = 20f;
        private const float MoveDuration = 1.2f;
        private const float RotateDuration = 1.5f;

        private Tween[] _moveTweens;
        private Tween[] _rotateTweens;
        private Vector2[] _initialPositions;
        private Quaternion[] _initialRotations;

        private void Start() => backButton.onClick.AddListener(BackToTabs);

        private void Awake()
        {
            _initialPositions = new Vector2[animatedObjects.Length];
            _initialRotations = new Quaternion[animatedObjects.Length];
            for (int i = 0; i < animatedObjects.Length; i++)
            {
                _initialPositions[i] = animatedObjects[i].anchoredPosition;
                _initialRotations[i] = animatedObjects[i].localRotation;
            }
        }

        private void OnEnable()
        {
            _moveTweens = new Tween[animatedObjects.Length];
            _rotateTweens = new Tween[animatedObjects.Length];

            for (int i = 0; i < animatedObjects.Length; i++)
            {
                var rect = animatedObjects[i];
                float delay = i * 0.15f;

                rect.anchoredPosition = _initialPositions[i];
                rect.localRotation = _initialRotations[i];

                _moveTweens[i] = rect.DOAnchorPosY(_initialPositions[i].y + MoveAmplitude, MoveDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(delay)
                    .SetUpdate(true);

                _rotateTweens[i] = rect.DOLocalRotate(
                        new Vector3(0, 0, RotateAmplitude),
                        RotateDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(delay)
                    .SetUpdate(true);
            }
        }

        private void OnDisable()
        {
            if (_moveTweens != null)
            {
                foreach (var t in _moveTweens)
                    t?.Kill();
            }

            if (_rotateTweens != null)
            {
                foreach (var t in _rotateTweens)
                    t?.Kill();
            }

            for (int i = 0; i < animatedObjects.Length; i++)
            {
                animatedObjects[i].anchoredPosition = _initialPositions[i];
                animatedObjects[i].localRotation = _initialRotations[i];
            }
        }

        private void BackToTabs() => _tabs.BackToMenu();
    }
}
