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

        private readonly float _moveAmplitude = 30f;
        private readonly float _rotateAmplitude = 20f;
        private readonly float _moveDuration = 1.2f;
        private readonly float _rotateDuration = 1.5f;

        private Tween[] _moveTweens;
        private Tween[] _rotateTweens;

        private void Start() => backButton.onClick.AddListener(BackToTabs);

        private void OnEnable()
        {
            _moveTweens = new Tween[animatedObjects.Length];
            _rotateTweens = new Tween[animatedObjects.Length];

            for (int i = 0; i < animatedObjects.Length; i++)
            {
                var rect = animatedObjects[i];
                float delay = i * 0.15f;

                _moveTweens[i] = rect.DOAnchorPosY(_moveAmplitude, _moveDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(delay)
                    .SetUpdate(true);

                _rotateTweens[i] = rect.DOLocalRotate(
                        new Vector3(0, 0, _rotateAmplitude),
                        _rotateDuration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetDelay(delay)
                    .SetUpdate(true);
            }
        }

        private void OnDisable()
        {
            if (_moveTweens != null)
                foreach (var t in _moveTweens)
                    t?.Kill();

            if (_rotateTweens != null)
                foreach (var t in _rotateTweens)
                    t?.Kill();
        }

        private void BackToTabs() => _tabs.BackToMenu();
    }
}
