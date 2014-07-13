using System;

namespace Mestevens.Injection.Core.Impl
{

	public class Binding
	{

		public Type Key { get; set; }
		public object Value { get; set; }
		public string Name { get; set; }
		public bool Singleton { get; set; }
		public int Strength { get; set; }

		public Binding(Type keyClazz)
		{
			this.Key = keyClazz;
			this.Value = null;
			this.Name = "";
			this.Singleton = false;
			this.Strength = 0;
		}

	}

}