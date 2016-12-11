using Zenject;

namespace johnfn {
    public class ZenjectInstaller: MonoInstaller {
        public override void InstallBindings() {
            Container.Bind<IUtil>().To<Util>().AsSingle();

            Container.Bind<ITimeManager>().To<TimeManager>().AsSingle();

            Container.Bind<IGroups>().To<Groups>().AsSingle();

            Container.Bind<IPrefabReferences>().To<PrefabReferences>().AsSingle();
        }
    }
}