using Miranda;

public class TestModule : Module {

	public override void Configure() {
		Bind<string>().To("A test string");
		Bind<string>().To("Named string").Named("test");
		Bind<int>().To(5);
		Bind<int>().To(76).Named("test");
		Bind<TestService>().AsSingleton();
		Bind<ITestInterface>().To<TestImplementation>();
	}

}
