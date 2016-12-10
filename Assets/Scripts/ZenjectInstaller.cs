using Zenject;

public class ZenjectInstaller: MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<string>().FromInstance("Hello World!");
        Container.Bind<IManager>().To<Manager>().AsSingle();
    }
}