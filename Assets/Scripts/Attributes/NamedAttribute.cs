using System;

namespace Miranda {

public class NamedAttribute : Attribute {

	public readonly string Name;

	public NamedAttribute(string name) {
		this.Name = name;
	}

}

}
