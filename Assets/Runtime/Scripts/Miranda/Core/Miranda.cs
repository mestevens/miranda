using System;

using Mestevens.Injection.Core.Api;
using Mestevens.Injection.Core.Impl;

namespace Mestevens.Injection.Core
{

	public static class Miranda
	{

		private class RootContext : Context
		{
			public RootContext() : base()
			{

			}

			public override void MapBindings()
			{

			}
		}

		public static Context Init(params Context[] contexts)
		{
			RootContext rootContext = new RootContext();
			rootContext.MapBindings();
			foreach(Context context in contexts)
			{
				rootContext.AddContext(context);
			}
			rootContext.InstantiateBindings();
			return rootContext;
		}

	}

}