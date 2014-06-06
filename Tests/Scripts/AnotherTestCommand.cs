using UnityEngine;
using System.Collections;

using Mestevens.Injection.Extensions;

public class AnotherTestCommand : Command {

	public override void MapParameters(params object[] parameters) {

	}

	public override void Execute() {
		Debug.Log ("AnotherTestExecute");
	}

}
