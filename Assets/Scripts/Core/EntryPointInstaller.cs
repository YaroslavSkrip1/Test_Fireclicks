using Config;
using Networking;
using UI;
using UnityEngine;
using Zenject;

namespace Core
{
    public class EntryPointInstaller : MonoInstaller
    {
        [SerializeField] private ProjectConfig _projectConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_projectConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<UnityWebRequester>().AsSingle();

            Container.Bind<TabsController>().FromComponentInHierarchy().AsSingle();

            Debug.Log("EntryPointInstaller bindings registered");
        }
    }
}