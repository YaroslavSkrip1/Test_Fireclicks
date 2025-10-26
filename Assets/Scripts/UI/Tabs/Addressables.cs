using System.Collections.Generic;
using Config;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI.Tabs
{
    public class Addressables : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [Inject] private ProjectConfig _config;

        [SerializeField] private Transform parent;
        private bool _injectDependencies = true;
        private bool _setActiveAfterSpawn = true;

        private readonly List<GameObject> _spawnedTabs = new();

        private async void Start()
        {
            await UnityEngine.AddressableAssets.Addressables.InitializeAsync().ToUniTask();
            await LoadAllTabs();
        }

        private async UniTask LoadAllTabs()
        {
            for (int i = 0; i < _config.TabKeys.Length; i++)
            {
                string key = _config.TabKeys[i];

                var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(key, parent ?? transform);
                var instance = await handle.ToUniTask();

                if (instance == null)
                {
                    Debug.LogError($"Failed to instantiate tab: {key}");
                    continue;
                }

                if (_injectDependencies) 
                    InjectDependencies(instance);

                instance.SetActive(_setActiveAfterSpawn);
                _spawnedTabs.Add(instance);
            }
        }

        private void InjectDependencies(GameObject instance)
        {
            SceneContext sceneContext = FindFirstObjectByType<SceneContext>();
            if (sceneContext != null)
                sceneContext.Container.InjectGameObject(instance);
            else if (_container != null) 
                _container.InjectGameObject(instance);
        }

        private void OnDestroy()
        {
            foreach (var tab in _spawnedTabs)
            {
                if (tab != null)
                    UnityEngine.AddressableAssets.Addressables.ReleaseInstance(tab);
            }

            _spawnedTabs.Clear();
        }
    }
}
