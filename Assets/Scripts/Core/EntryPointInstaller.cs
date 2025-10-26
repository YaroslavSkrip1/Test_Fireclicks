using Config;
using Networking;
using UI;
using UI.Tabs.Factory;
using UI.Tabs.Factory.Impls;
using UnityEngine;
using Zenject;

namespace Core
{
    public class EntryPointInstaller : MonoInstaller
    {
        [SerializeField] private ProjectConfig projectConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(projectConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<UnityWebRequester>().AsSingle();

            Container.Bind<ITabsFactory>().To<TabsFactory>().AsSingle();
            Container.Bind<TabsController>().FromComponentInHierarchy().AsSingle();
        }
    }
}