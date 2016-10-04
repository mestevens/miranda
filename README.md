# Miranda

Dependency injection framework for unity.

## Usage

### Create Modules with Bindings

```
using Miranda;

public class TestModule : Module {

    public override void Configure() {
        Bind<string>().To("A test string");
        Bind<string>().To("A named string").Named("test");
        Bind<IInterfaceClass>().To<ImplementationClass>().AsSingleton();
    }

}
```

### Create the injector and inject it into the scene

```
using UnityEngine;
using Miranda;

public class TestBehaviour : MonoBehaviour {

    void Start() {
        Injection.CreateStaticInjector(new TestModule());
        Injection.InjectIntoScene();
    }

}
```

Now any Scripts that are in the scene will be injected into (like the following):
```
using UnityEngine;
using Miranda;

public class AnotherBehaviour : MonoBehaviour {

    [Inject("test)]
    public string TestString { get; set; }

}
```

You can also used named injection into constructors/methods using the `Named` attribute
```
using Miranda;

public class TestService {

    [Inject]
    public TestService([Named("test")] string text) {
        //do whatever
    }

}
```

Note: As of right now, this will not inject into child game objects