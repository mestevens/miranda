using System;

namespace Mestevens.Injection.Core.Exceptions.Impl
{

	public class BindingNotFoundException : Exception
	{

		public Type BaseClass { get; set; }

		public BindingNotFoundException(string message) : base(message)
		{
			BaseClass = null;
		}

	}

}