using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI.Tabs.Factory.Impls;
using UnityEngine;
using Zenject;

namespace UI.Tabs.Factory
{
    public class TabsFactory : ITabsFactory
    {
        private readonly DiContainer _container;
        private readonly Dictionary<string, GameObject> _cache = new();

        public TabsFactory(DiContainer container) => _container = container;

        public async UniTask<GameObject> CreateTab(string key, Transform parent)
        {
            if (_cache.TryGetValue(key, out var existing))
                return existing;

            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(key, parent);
            var instance = await handle.ToUniTask();

            instance.SetActive(false);
            _container.InjectGameObject(instance);
            _cache[key] = instance;

            return instance;
        }
    }
}