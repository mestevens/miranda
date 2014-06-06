using System;
using System.Reflection;

using Mestevens.Injection.Core.Api;
using Binder = Mestevens.Injection.Core.Impl.Binder;

namespace Mestevens.Injection.Core
{

	public abstract class Context
	{

		public IBinder injectionBinder;
	
		public Context()
		{
			injectionBinder = new Binder();
		}
	
		public abstract void MapBindings();
		
		public IBinder Bind<T>()
		{
			return injectionBinder.Bind<T>();
		}

		public IBinder BindSignal<T>()
		{
			return injectionBinder.BindSignal<T>();
		}
		
		public T Get<T>(string name = "")
		{
			object instance = injectionBinder.Get<T>(name);
			return (T)instance;
		}
		
		public void AddContext(Context otherContext)
		{
			if (otherContext.injectionBinder.IsEmpty()) {
				otherContext.MapBindings();
			}
			injectionBinder.AddBinder(otherContext.injectionBinder);
		}

		public void InstantiateBindings()
		{
			injectionBinder.InstantiateBindings();
		}

	}

}