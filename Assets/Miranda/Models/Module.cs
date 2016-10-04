namespace Miranda {

public abstract class Module
{
	
    private Injector Injector { get; set; }

    public void Configure(Injector injector) {
        this.Injector = injector;
        Configure();
    }

    public abstract void Configure();

    public Binding Bind<T>() {
        return this.Injector.Bind<T>();
    }

}

}
