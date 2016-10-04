using System;

namespace Miranda {

public class InjectAttribute : Attribute {

	public readonly string Name;

	public InjectAttribute() {
		this.Name = null;
	}

	public InjectAttribute(string named) {
		this.Name = named;
	}

}

}
