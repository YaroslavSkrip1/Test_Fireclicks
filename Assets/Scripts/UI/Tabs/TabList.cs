using System.Collections.Generic;
using UI.Item;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Tabs
{
    public class TabList : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Button backButton;

        private const int TotalCount = 1000;
        private const int VisibleCount = 30;
        private readonly List<ItemView> _pool = new();

        [Inject] private TabsController _tabs;

        private float _itemHeight;
        private float _spacing;
        private bool _initialized;

        private void Awake()
        {
            var rect = _itemPrefab.GetComponent<RectTransform>();
            _itemHeight = rect.sizeDelta.y;
            _spacing = 4f;

            for (int i = 0; i < VisibleCount; i++)
            {
                var go = Instantiate(_itemPrefab, _content);
                var view = go.GetComponent<ItemView>();
                _pool.Add(view);
            }

            FixContentHeight();

            _scroll.onValueChanged.AddListener(OnScroll);
        }

        private void Start()
        {
            backButton.onClick.AddListener(BackToTabs);
            _initialized = true;

            Canvas.ForceUpdateCanvases();
            _scroll.verticalNormalizedPosition = 1f;

            UpdateVisibleItems();
        }

        private void OnDestroy() => _scroll.onValueChanged.RemoveListener(OnScroll);

        private void OnScroll(Vector2 pos)
        {
            if (_initialized)
                UpdateVisibleItems();
        }

        private void FixContentHeight()
        {
            float fullHeight = TotalCount * (_itemHeight + _spacing);
            _content.anchorMin = new Vector2(0, 1);
            _content.anchorMax = new Vector2(1, 1);
            _content.pivot = new Vector2(0.5f, 1);
            _content.anchoredPosition = Vector2.zero;
            _content.sizeDelta = new Vector2(0, fullHeight);

            Canvas.ForceUpdateCanvases();
            _scroll.verticalNormalizedPosition = 1f;
        }

        private void UpdateVisibleItems()
        {
            float scrollY = _content.anchoredPosition.y;
            int firstVisibleIndex = Mathf.FloorToInt(scrollY / (_itemHeight + _spacing));
            firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, TotalCount - VisibleCount);

            for (int i = 0; i < _pool.Count; i++)
            {
                int index = firstVisibleIndex + i;
                if (index >= TotalCount)
                {
                    _pool[i].gameObject.SetActive(false);
                    continue;
                }

                _pool[i].gameObject.SetActive(true);
                _pool[i].SetIndex(index);

                var rt = _pool[i].GetComponent<RectTransform>();
                float yPos = -index * (_itemHeight + _spacing);
                rt.anchoredPosition = new Vector2(0, yPos);
            }
        }

        public void BackToTabs() => _tabs.BackToMenu();
    }
}
