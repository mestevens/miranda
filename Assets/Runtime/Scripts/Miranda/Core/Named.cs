using System;

namespace Mestevens.Injection.Core
{

	public class Named : Attribute
	{

		public string Name { get; set; }

		public Named(string name)
		{
			this.Name = name;
		}

	}

}