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
		
		public virtual Context Bind<T>()
		{
			this.injectionBinder.Bind<T>();
			return this;
		}

		public virtual Context To<T>()
		{
			this.injectionBinder.To<T>();
			return this;
		}

		public virtual Context To(object obj)
		{
			this.injectionBinder.To(obj);
			return this;
		}

		public virtual Context ToSingleton()
		{
			this.injectionBinder.ToSingleton();
			return this;
		}

		public virtual Context Named(string name)
		{
			this.injectionBinder.Named(name);
			return this;
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
			otherContext.MapPluginBindings();
			injectionBinder.AddBinder(otherContext.injectionBinder);
		}

		protected virtual void MapPluginBindings()
		{

		}

		public void InstantiateBindings()
		{
			injectionBinder.InstantiateBindings();
		}

	}

}