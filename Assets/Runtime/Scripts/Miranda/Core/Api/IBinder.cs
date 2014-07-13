using System;
using System.Collections.Generic;

using Mestevens.Injection.Core.Impl;

namespace Mestevens.Injection.Core.Api
{

	public interface IBinder
	{
		IBinder Bind<T>();

		IBinder To<T>();

		Binder To(object obj);
		
		IBinder Named(string name);
		
		IBinder ToSingleton();

		IBinder WithStrength(int strength);
		
		object Get<T>(string name = "");

		object Get(Type type, string name = "");

		void AddBinder(IBinder otherBinder);

		bool IsEmpty();

		void InstantiateBindings();

		IDictionary<object, IList<Binding>> GetBindings();

		IDictionary<object, IList<CachedBinding>> GetCachedBindings();

		IDictionary<string, object> GetSingletons();

	}

}