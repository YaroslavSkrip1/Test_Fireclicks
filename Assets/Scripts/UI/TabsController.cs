using System.Collections.Generic;
using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class TabsController : MonoBehaviour
    {
        [SerializeField] private Button listButton;
        [SerializeField] private Button timeButton;
        [SerializeField] private Button animatedButton;
        [SerializeField] private Button requestButton;

        [Inject] private ProjectConfig _config;
        [Inject] private DiContainer _container;

        private readonly List<GameObject> _loadedTabs = new();
        private GameObject _activeTab;
        private Transform _root;

        private async void Start()
        {
            listButton.onClick.AddListener(OpenList);
            timeButton.onClick.AddListener(OpenTime);
            animatedButton.onClick.AddListener(OpenAnimated);
            requestButton.onClick.AddListener(OpenRequest);

            await Initialize();
        }

        private async UniTask Initialize()
        {
            _root = FindObjectOfType<Canvas>().transform;
            await LoadTabsAsync();
        }

        private async UniTask LoadTabsAsync()
        {
            foreach (var key in _config.TabKeys)
            {
                var handle = Addressables.InstantiateAsync(key, _root);
                var instance = await handle.ToUniTask();

                instance.SetActive(false);

                _container.InjectGameObject(instance);

                _loadedTabs.Add(instance);
            }
        }

        private void SwitchToTab(int index)
        {
            if (index < 0 || index >= _loadedTabs.Count)
                return;

            gameObject.SetActive(false);

            if (_activeTab != null)
                _activeTab.SetActive(false);

            _activeTab = _loadedTabs[index];
            _activeTab.SetActive(true);
        }

        public void OpenList() => SwitchToTab(0);
        public void OpenTime() => SwitchToTab(1);
        public void OpenAnimated() => SwitchToTab(2);
        public void OpenRequest() => SwitchToTab(3);

        public void BackToMenu()
        {
            foreach (var tab in _loadedTabs)
                tab.SetActive(false);

            gameObject.SetActive(true);
        }
    }
}
