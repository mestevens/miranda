using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

using Mestevens.Injection.Core.Api;
using Mestevens.Injection.Core.Exceptions.Impl;

using Mestevens.Injection.Extensions;

namespace Mestevens.Injection.Core.Impl
{

	public class Binder : IBinder
	{

		private IDictionary<object, IList<Binding>> binder;
		private IDictionary<object, IList<CachedBinding>> cachedBinder;
		private static IDictionary<string, object> singletons;
		private IDictionary<Type, IList<Type>> signalsToCommands;

		private Binding binding;
		private Type signal;

		private IList<BindingNotFoundException> exceptions;

		public Binder()
		{
			binder = new Dictionary<object, IList<Binding>>();
			cachedBinder = new Dictionary<object, IList<CachedBinding>>();
			singletons = new Dictionary<string, object>();
			exceptions = new List<BindingNotFoundException>();
			signalsToCommands = new Dictionary<Type, IList<Type>>();
		}

		public IBinder Bind<T>()
		{
			return Bind(typeof(T));
		}

		private Binder Bind(Type keyClazz)
		{
			binding = new Binding(keyClazz);
			return this;
		}

		public IBinder BindSignal<T>()
		{
			return BindSignal(typeof(T));
		}

		private Binder BindSignal(Type keyClazz)
		{
			signal = keyClazz;
			if (!signalsToCommands.ContainsKey(keyClazz))
			{
				return this.Bind(keyClazz).To(keyClazz).ToSingleton() as Binder;
			}
			return this;
		}

		public IBinder ToCommand<T>()
		{
			return ToCommand(typeof(T));
		}

		private Binder ToCommand(Type type)
		{
			if (signalsToCommands.ContainsKey(signal))
			{
				signalsToCommands[signal].Add(type);
			}
			else
			{
				IList<Type> commandList = new List<Type>();
				commandList.Add(type);
				signalsToCommands.Add(signal, commandList);
			}
			return this.Bind(type).To(type);
		}

		public IBinder To<T>()
		{
			return To(typeof(T));
		}

		public Binder To(object obj)
		{
			if (binding.Value != null)
			{
				binder[binding.Key][binder[binding.Key].Count - 1].Value = obj;
			}
			else
			{
				binding.Value = obj;
				AddToBinder(binding);
			}
			return this;
		}

		public IBinder Named(string name)
		{
			if (binding.Value != null)
			{
				binder[binding.Key][binder[binding.Key].Count - 1].Name = name;
			}
			else
			{
				binding.Name = name;
			}
			return this;
		}

		public IBinder ToSingleton()
		{
			if (binding.Value != null)
			{
				binder[binding.Key][binder[binding.Key].Count - 1].Singleton = true;
			}
			else
			{
				binding.Singleton = true;
				binding.Value = binding.Key;
				AddToBinder(binding);
			}
			return this;
		}

		public object Get<T>(string name = "")
		{
			return this.Get(typeof(T), name, true);
		}

		public object Get(Type type, string name = "")
		{
			return this.Get(type, name, true);
		}

		private object GetCachedBinding(Type keyClazz, string name = "", bool throwExceptions = true)
		{
			IList<CachedBinding> cachedBindings = cachedBinder[keyClazz];
			foreach (CachedBinding cachedBinding in cachedBindings)
			{
				if (cachedBinding.Name.Equals(name))
				{
					if (cachedBinding.Singleton)
					{
						return singletons[cachedBinding.Type.ToString() + "_" + cachedBinding.Name];
					}
					object cachedInstance = cachedBinding.Activate();
					SetMemberInfo(cachedInstance.GetType().GetProperties(), typeof(PropertyInfo), ref cachedInstance, throwExceptions);
					SetMemberInfo(cachedInstance.GetType().GetFields(), typeof(FieldInfo), ref cachedInstance, throwExceptions);
					return cachedInstance;
				}
			}
			throw new Exception();
		}

		private object Get(Type keyClazz, string name = "", bool throwExceptions = true)
		{
			if (keyClazz.Equals (typeof(IBinder)))
			{
				return this;
			}
			try
			{
				//Check to see if the instance is cached
				try
				{
					return GetCachedBinding(keyClazz, name, throwExceptions);
				}
				catch (Exception)
				{
				}

				IList<Binding> bindings;
				try
				{
					bindings = binder[keyClazz];
				}
				catch (KeyNotFoundException ex)
				{
					if (!keyClazz.IsInterface)
					{
						bindings = new List<Binding>();
						Binding bin = new Binding(keyClazz);
						bin.Value = keyClazz;
						bindings.Add(bin);
						binder[keyClazz] = bindings;
					}
					else
					{
						throw ex;
					}
				}

				foreach (Binding namedBinding in bindings)
				{
					if (namedBinding.Name.Equals(name))
					{
						Type type = namedBinding.Value as Type;
						if (type == null)
						{
							//return namedBinding.Value;
							CachedBinding b = new CachedBinding(keyClazz, namedBinding.Value, name);
							//AddToCachedBinder(keyClazz, b);
							return b.Activate();
						}
						if(Convert.GetTypeCode(namedBinding.Value) != TypeCode.Object)
						{
							CachedBinding b = new CachedBinding(type, namedBinding.Value, name);
							AddToCachedBinder(keyClazz, b);
							return b.Activate();
						}
						//Reflect the constructor
						ConstructorInfo[] constructorInfoArray = type.GetConstructors();
						foreach (ConstructorInfo constructorInfo in constructorInfoArray)
						{
							foreach (Attribute attr in constructorInfo.GetCustomAttributes(false))
							{
								if (attr is Inject)
								{
									ParameterInfo[] parameterInfoArray = constructorInfo.GetParameters();
									object[] paramArray = new object[parameterInfoArray.Length];
									int i = 0;
									foreach(ParameterInfo parameterInfo in parameterInfoArray)
									{
										Type parameterType = parameterInfo.ParameterType;
										string parameterName = "";
										foreach(Attribute parameterAttrs in parameterInfo.GetCustomAttributes(false))
										{
											if (parameterAttrs is Named)
											{
												Named namedAttr = parameterAttrs as Named;
												parameterName = namedAttr.Name;
											}
										}
										paramArray[i] = this.Get(parameterType, parameterName, throwExceptions);
										if (paramArray[i] == null)
										{
											exceptions[exceptions.Count - 1].BaseClass = type;
										}
										i++;
									}

									//Cache and return the instance
									CachedBinding cachedBinding = new CachedBinding(type, paramArray, name);
									cachedBinding.Singleton = namedBinding.Singleton;

									return GetInstanceAndCache(keyClazz, cachedBinding, throwExceptions);
								}
							}
						}
						//Cache and return the instance
						CachedBinding cached = new CachedBinding(type, name);
						cached.Singleton = namedBinding.Singleton;

						return GetInstanceAndCache(keyClazz, cached, throwExceptions);

					}

				}
				throw new Exception();
			}
			catch (Exception)
			{
				//Set-up the exception
				BindingNotFoundException bindingNotFoundException;
				if (name.Equals(""))
				{
					bindingNotFoundException = new BindingNotFoundException("Binding not found for " + keyClazz.ToString() + ".");
				}
				else
				{
					bindingNotFoundException = new BindingNotFoundException("Binding named " + name + " not found for " + keyClazz.ToString() + ".");
				}
				if (throwExceptions)
				{
					throw bindingNotFoundException;
				}
				else
				{
					exceptions.Add(bindingNotFoundException);
					return null;
				}
			}
		}

		private object GetInstanceAndCache(Type keyClazz, CachedBinding cachedBinding, bool throwExceptions = false)
		{

			AddToCachedBinder(keyClazz, cachedBinding);

			object noParamInstance = cachedBinding.Activate();

			//UnityEngine.Debug.Log (cachedBinding.Value.GetType ());
			//if (cachedBinding.Value.GetType()
			//{
				SetMemberInfo (noParamInstance.GetType ().GetProperties (), typeof(PropertyInfo), ref noParamInstance, throwExceptions);
				SetMemberInfo (noParamInstance.GetType ().GetFields (), typeof(FieldInfo), ref noParamInstance, throwExceptions);
			//}

			if (cachedBinding.Singleton)
			{
				singletons.Add(cachedBinding.Type.ToString() + "_" + cachedBinding.Name, noParamInstance);
				noParamInstance = singletons[cachedBinding.Type.ToString() + "_" + cachedBinding.Name];
			}

			return noParamInstance;
		}

		public void AddBinder(IBinder otherBinder)
		{
			//Figure out a way to refactor this so it looks nicer
			foreach(KeyValuePair<object, IList<Binding>> otherPair in otherBinder.GetBindings())
			{
				if (!binder.ContainsKey(otherPair.Key))
				{
					binder.Add(otherPair);
				}
				else
				{
					foreach(Binding otherBinding in otherPair.Value)
					{
						if(!binder[otherPair.Key].Contains(otherBinding))
						{
							binder[otherPair.Key].Add(otherBinding);
						}
					}
				}
			}
			foreach(KeyValuePair<object, IList<CachedBinding>> otherCachePair in otherBinder.GetCachedBindings())
			{
				if (!cachedBinder.ContainsKey(otherCachePair.Key))
				{
					cachedBinder.Add(otherCachePair);
				}
				else
				{
					foreach(CachedBinding otherCachedBinding in otherCachePair.Value)
					{
						if(!cachedBinder[otherCachePair.Key].Contains(otherCachedBinding))
						{
							cachedBinder[otherCachePair.Key].Add(otherCachedBinding);
						}
					}
				}
			}
			foreach(KeyValuePair<Type, IList<Type>> otherSignalsToCommands in otherBinder.GetSignalsToCommands())
			{
				if (!signalsToCommands.ContainsKey(otherSignalsToCommands.Key))
				{
					signalsToCommands.Add(otherSignalsToCommands);
				}
				else
				{
					foreach(Type otherType in otherSignalsToCommands.Value)
					{
						if(!signalsToCommands[otherSignalsToCommands.Key].Contains(otherType))
						{
							signalsToCommands[otherSignalsToCommands.Key].Add(otherType);
						}
					}
				}
			}
		}

		public bool IsEmpty()
		{
			return (binder.Count == 0);
		}

		//Handle Property/Member injection
		//See if this can be extended to handle constructor injection as well
		private void SetMemberInfo(MemberInfo[] memberInfoArray, Type type, ref object instance, bool throwExceptions)
		{
			foreach(MemberInfo memberInfo in memberInfoArray)
			{
				foreach(Attribute attr in memberInfo.GetCustomAttributes(false))
				{
					if (attr is Inject)
					{
						Inject injectAttr = attr as Inject;
						if (type.Equals(typeof(FieldInfo)))
						{
							FieldInfo fieldInfo = memberInfo as FieldInfo;
							object valueInstance = Get (fieldInfo.FieldType, injectAttr.name, throwExceptions);
							if (valueInstance == null)
							{
								exceptions[exceptions.Count - 1].BaseClass = instance.GetType();
							}
							fieldInfo.SetValue(instance, valueInstance);
						}
						else if (type.Equals(typeof(PropertyInfo)))
						{
							PropertyInfo propertyInfo = memberInfo as PropertyInfo;
							object valueInstance = Get(propertyInfo.PropertyType, injectAttr.name, throwExceptions);
							if (valueInstance == null)
							{
								exceptions[exceptions.Count - 1].BaseClass = instance.GetType();
							}
							propertyInfo.SetValue(instance, valueInstance, null);
						}
					}
				}
			}
		}

		private void AddToBinder(Binding binding)
		{
			if (binder.ContainsKey(binding.Key))
			{
				binder[binding.Key].Add(binding);
			}
			else
			{
				IList<Binding> list = new List<Binding>();
				list.Add(binding);
				binder.Add(binding.Key, list);
			}
		}

		private void AddToCachedBinder(Type keyClazz, CachedBinding cachedBinding)
		{
			if (cachedBinder.ContainsKey(keyClazz))
			{
				cachedBinder[keyClazz].Add(cachedBinding);
			}
			else
			{
				IList<CachedBinding> list = new List<CachedBinding>();
				list.Add(cachedBinding);
				cachedBinder.Add(keyClazz, list);
			}
		}

		public void InstantiateBindings()
		{
			foreach(IList<Binding> bindingList in binder.Values)
			{
				foreach(Binding binding in bindingList)
				{
					this.Get(binding.Key, binding.Name, false);
				}
			}
			if (exceptions.Count != 0)
			{
				IList<string> previousString = new List<string>();
				string exceptionString = "Bindings not found for the following:\n";

				for (int i = 0; i < exceptions.Count; i++)
				{
					BindingNotFoundException bindingNotFoundException = exceptions[i];
					if (!previousString.Contains(bindingNotFoundException.Message))
					{
						previousString.Add(bindingNotFoundException.Message);
						exceptionString += bindingNotFoundException.Message + "\n";
						for (int x = i; x < exceptions.Count; x++)
						{
							BindingNotFoundException nextException = exceptions[x];
							if (bindingNotFoundException.Message.Equals(nextException.Message))
							{
								if (nextException.BaseClass != null)
								{
									exceptionString += "\tin class " + nextException.BaseClass + "\n";
								}
							}
						}

					}
				}
				throw new BindingNotFoundException(exceptionString);
			}
			//Initialize commands
			foreach(KeyValuePair<Type, IList<Type>> pairs in signalsToCommands)
			{
				Signal s = (Signal)this.Get(pairs.Key, "", true);
				foreach(Type commands in pairs.Value)
				{
					s.AddCommand(commands);
				}
			}
		}

		public IDictionary<object, IList<Binding>> GetBindings()
		{
			return binder;
		}
		
		public IDictionary<object, IList<CachedBinding>> GetCachedBindings()
		{
			return cachedBinder;
		}
		
		public IDictionary<string, object> GetSingletons()
		{
			return singletons;
		}
		
		public IDictionary<Type, IList<Type>> GetSignalsToCommands()
		{
			return signalsToCommands;
		}

	}

}