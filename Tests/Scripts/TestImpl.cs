using UnityEngine;
using System.Collections;

public class TestImpl : ITest {

	private string message;

	public TestImpl() {
		message = "hello";
	}

	public string Print() {
		Debug.Log (message);
		return message;
	}

}
