using UnityEngine;
using System.Collections;

public class AnotherTestImpl : ITest {

	public string Print() {
		string text = "ANOTHER PRINT";
		Debug.Log (text);
		return text;
	}
}
