using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI.Tabs.Factory.Impls
{
    public interface ITabsFactory
    {
        UniTask<GameObject> CreateTab(string key, Transform parent);
    }
}