using UnityEngine;
using System.Collections;

using Mestevens.Injection.Extensions;

public class TestCommand : Command {

	public TestCommand() {

	}

	public override void MapParameters(params object[] parameters) {
		
	}

	public override void Execute() {
		Debug.Log("Execute");
	}

}
