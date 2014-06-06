# Miranda

Miranda is a dependency injection framework for .NET 2.0 and was primary created due to the lack of frameworks for this version of .NET. For more information about dependency injection and inversion of control (IoC) see [Wikipedia](http://en.wikipedia.org/wiki/Dependency_injection).

Miranda was originally designed to be used with Unity, but is versatile enough to be used in any C# project.


## Set-up

Set-up for Miranda is very easy, simply copy the DLL included in the master branch into your project, and you have access to all that Miranda has to offer.

## Usage

In order to use Miranda, you first need to have a class that requires dependency injection. Miranda can inject into the following parts of a class:

* Constructor
* Property
* Public member field

In order to inject into these, simple add the `[Inject]` attribute to the desired part of the class. For example, in order to inject an IRequestFactory into a class you can do it in one of three ways:

	using Mestevens.Injection.Core;

	public class ExampleClass
	{
	
		[Inject]
		public IRequestFactory requestFactory;
		
		[Inject]
		public IRequestFactory RequestFactory { get; set; }
		
		[Inject]
		public ExampleClass(IRequestFactory requestFactory)
		{
			this.requestFactory = requestFactory;
		}
		
	}
	
Now in order to tell Miranda what to inject when an IRequestFactory is requested, you need to create a context such as the one below:

	using Mestevens.Injection.Core;
	
	public class ExampleContext : Context
	{
	
		public ExampleContext() : base()
		{
		
		}
		
		public override void MapBindings()
		{
		
			Bind<IRequestFactory>().To<DefaultRequestFactory>();
		
		}
	
	}

Once this is done, all you have to do is create the context, initialize it and request a instance of the class you want. 

The recommended way to instantiate your contexts is to use the Miranda static class. Doing it this way can be done like so:

	using Mestevens.Injection.Core
	
	...

	//Miranda.Init returns an initialized context so you can use that directly if you'd prefer
	Context context = Miranda.Init(new ExampleContext(), new AnotherContext());
	context.Get<ExampleClass>();

If you want a bit more control over your contexts, you can create/map/instantiate your contexts by yourself. For example:

	ExampleContext example = new ExampleContext();
	example.MapBindings();
	//InstantiateBindings is not needed but if you instantiate bindings you will be able to figure out what isn't bound, and all your bindings will be cached
	example.InstantiateBindings();
	ExampleClass exampleClass = example.Get<ExampleClass>();
	
The example class will now have the `DefaultRequestFactory` class specified in the context.

You can also add other contexts to your context by doing the following before you instantiate your bindings:

	AnotherContext anotherContext = new AnotherContext();
	example.AddContext(anotherContext);
	
At the moment Miranda will ignore any duplicate bindings found in the child contexts, but there are plans to implement a priority binding system.
	
## Named Injection 

Miranda can also provide a method to do named injection. When specifying the `[Inject]` attribute, you can also request the name of the binding you want using `[Inject("name")]` and when binding your classes, just make sure to do the following:

	Bind<Class>().To<AnotherClass>().ToName("name");
	
You can also do named injection in constructors if you want

	private Class myClass;

	[Inject]
	public MyConstructor([Named("name")] Class myClass)
	{
		//You can then assign values to private variables
		this.myClass = myClass
	}
	
	
## Singleton Injection

When binding classes you can bind them to singletons by doing the following:

	Bind<IClass>().To<Class>().ToSingleton();
	
Or you can just bind a class as a singleton:

	Bind<Class>().ToSingleton();
	
Additionally, if you bind a class to a specifc object, it is automatically bound as a singleton

	Class instanceClass = new Class();
	instanceClass.Property = "a value";
	
	Bind<IClass>().To(instanceClass);
