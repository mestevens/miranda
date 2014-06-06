using System;

namespace Mestevens.Injection.Core.Impl
{
	
	public class CachedBinding
	{
		
		public Type Type { get; set; }
		public object Value { get; set; }
		public object[] ParamArray { get; set; }
		public string Name { get; set; }
		public bool Singleton { get; set; }
		
		public CachedBinding(Type type, object[] ParamArray, string name = "")
		{
			this.Type = type;
			this.Value = null;
			this.ParamArray = ParamArray;
			this.Name = name;
		}

		public CachedBinding(Type type, object value, string name = "")
		{
			this.Type = type;
			this.Value = value;
			this.ParamArray = null;
			this.Name = name;
		}

		public CachedBinding(Type type, string name = "")
		{
			this.Type = type;
			this.Value = null;
			this.ParamArray = null;
			this.Name = name;
		}

		public object Activate()
		{
			if (this.Value != null)
			{
				return this.Value;
			}
			else if (ParamArray == null)
			{
				return Activator.CreateInstance(Type);
			}
			return Activator.CreateInstance(Type, ParamArray, null);
		}

	}
	
}