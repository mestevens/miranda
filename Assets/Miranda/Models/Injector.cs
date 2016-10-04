using System;
using System.Reflection;
using System.Collections.Generic;

namespace Miranda {

public class Injector {

	private IDictionary<Type, IList<Binding>> bindings;

	public Injector() {
		bindings = new Dictionary<Type, IList<Binding>>();
	}

	public Binding Bind<T>() {
		Type type = typeof(T);
		Binding binding = new Binding(type);
		if (!bindings.ContainsKey(type)) {
			bindings.Add(type, new List<Binding>());
		}
		bindings[type].Add(binding);
		return binding;
	}

	public Object GetInstance<T>() {
		return GetInstance<T>(null);
	}

	public Object GetInstance<T>(string name) {
		return GetInstance(typeof(T), name);
	}

	public Object GetInstance(Type type) {
		return GetInstance(type, null);
	}

	public Object GetInstance(Type type, string name) {
		if (bindings.ContainsKey(type)) {
			IList<Binding> bindingList = bindings[type];
			Binding binding = null;
			foreach (Binding b in bindingList) {
				if (String.Equals(b.Name, name)) {
					binding = b;
				}
			}
			if (binding == null) {

			} else {
				if (binding.Instance == null) {
					Object obj = ConstructObject(binding.ImplementationType);
					InjectIntoObject(obj);
					if (binding.Singleton) {
						binding.Instance = obj;
					}
					return obj;
				} else {
					return binding.Instance;
				}
			}
		} else {
			Object obj = ConstructObject(type);
			InjectIntoObject(obj);
			return obj;
		}
		return null;
	}

	public void InjectIntoObject(Object sourceObject) {
		Type type = sourceObject.GetType();
		InjectIntoMethods(sourceObject, type);
		InjectIntoProperties(sourceObject, type);
	}

	private void InjectIntoProperties(Object sourceObject, Type type) {
		PropertyInfo[] properties = type.GetProperties();
		foreach (PropertyInfo property in properties) {
			if (Attribute.IsDefined(property, typeof(InjectAttribute))) {
				InjectAttribute injectAttribute = (property.GetCustomAttributes(typeof(InjectAttribute), false) as InjectAttribute[])[0];
				string name = injectAttribute.Name;
				Object obj = GetInstance(property.PropertyType, name);
				property.SetValue(sourceObject, obj, null);
			}
		}
	}

	private void InjectIntoMethods(Object sourceObject, Type type) {
		MethodInfo[] methods = type.GetMethods();
		foreach (MethodInfo method in methods) {
			if (Attribute.IsDefined(method, typeof(InjectAttribute))) {
				ParameterInfo[] methodParams = method.GetParameters();
				Object[] paramObjects = new Object[methodParams.Length];
				int i = 0;
				foreach (ParameterInfo methodParam in methodParams) {
					string name = null;
					if (Attribute.IsDefined(methodParam, typeof(NamedAttribute))) {
						NamedAttribute namedAttribute = (methodParam.GetCustomAttributes(typeof(NamedAttribute), false) as NamedAttribute[])[0];
						name = namedAttribute.Name;
					}
					paramObjects[i] = GetInstance(methodParam.ParameterType, name);
					i++;
				}
				method.Invoke(sourceObject, paramObjects);
			}
		}
	}

	private Object ConstructObject(Type type) {
		ConstructorInfo[] constructors = type.GetConstructors();
		foreach (ConstructorInfo constructor in constructors) {
			if (Attribute.IsDefined(constructor, typeof(InjectAttribute))) {
				ParameterInfo[] constructorParams = constructor.GetParameters();
				Object[] paramObjects = new Object[constructorParams.Length];
				int i = 0;
				foreach (ParameterInfo constructorParam in constructorParams) {
					string name = null;
					if (Attribute.IsDefined(constructorParam, typeof(NamedAttribute))) {
						NamedAttribute namedAttribute = (constructorParam.GetCustomAttributes(typeof(NamedAttribute), false) as NamedAttribute[])[0];
						name = namedAttribute.Name;
					}
					paramObjects[i] = GetInstance(constructorParam.ParameterType, name);
					i++;
				}
				return constructor.Invoke(paramObjects);
			}
		}
		return type.GetConstructor(Type.EmptyTypes).Invoke(new Object[0]);
	}

}

}
