using System.Collections.Generic;
using Config;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UI.Tabs.Factory.Impls;

namespace UI
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private Button listButton;
        [SerializeField] private Button timeButton;
        [SerializeField] private Button animatedButton;
        [SerializeField] private Button requestButton;

        private ITabsFactory _factory;
        private ProjectConfig _config;

        private readonly List<GameObject> _loadedTabs = new();
        private GameObject _activeTab;
        private Transform _root;

        [Inject]
        public void Construct(ProjectConfig config, ITabsFactory factory)
        {
            _config = config;
            _factory = factory;
        }

        private void Start()
        {
            listButton.onClick.AddListener(() => OpenTab(0));
            timeButton.onClick.AddListener(() => OpenTab(1));
            animatedButton.onClick.AddListener(() => OpenTab(2));
            requestButton.onClick.AddListener(() => OpenTab(3));

            _root = FindObjectOfType<Canvas>().transform;
        }

        private async void OpenTab(int index)
        {
            if (index < 0 || index >= _config.TabKeys.Length)
                return;

            string key = _config.TabKeys[index];

            var tab = await _factory.CreateTab(key, _root);
            if (tab == null)
                return;

            if (_activeTab != null) _activeTab.SetActive(false);

            _activeTab = tab;
            _activeTab.SetActive(true);

            gameObject.SetActive(false);

            if (!_loadedTabs.Contains(tab))
                _loadedTabs.Add(tab);
        }

        public void BackToMenu()
        {
            foreach (var tab in _loadedTabs) 
                tab.SetActive(false);

            _activeTab = null;
            gameObject.SetActive(true);
        }
    }
}
