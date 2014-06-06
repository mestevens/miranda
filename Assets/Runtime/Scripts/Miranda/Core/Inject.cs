using System;

namespace Mestevens.Injection.Core
{

	public class Inject : Attribute
	{

		public string name;

		public Inject()
		{
			this.name = "";
		}

		public Inject(string name)
		{
			this.name = name;
		}

	}

}