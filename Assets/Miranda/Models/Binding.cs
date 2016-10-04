using System;

namespace Miranda {

public class Binding {

	public Type Type { get; set; }

	public Type ImplementationType { get; set; }

	public string Name { get; set; }

	public bool Singleton { get; set; }

	public Object Instance { get; set; }

	public Binding(Type type) {
		Type = type;
		ImplementationType = type;
		Name = null;
		Singleton = false;
		Instance = null;
	}

	public Binding To<T>() {
		ImplementationType = typeof(T);
		return this;
	}

	public Binding To(Object instance) {
		Instance = instance;
		return this;
	}

	public Binding AsSingleton() {
		Singleton = true;
		return this;
	}

	public Binding Named(string name) {
		Name = name;
		return this;
	}

}

}