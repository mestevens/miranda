using UnityEngine;
using System.Collections;
using System;

using Mestevens.Injection.Core;

public class TestAnnotation : TestClass {

	[Inject("inject.inject")]
	public string name;

	public TestAnnotation() {
		name = "woah";
	}

	public TestAnnotation(string name) {
		this.name = name;
	}

	public void PrintName() {
		Debug.Log (name);
	}

	public void ChangeName(string name) {
		this.name = name;
	}


}
