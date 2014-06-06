using UnityEngine;
using System.Collections;

using Mestevens.Injection.Core;

public class ObjectInjectImpl : IObjectInject {

	private TestClass test3;
	private ITest test5;
	private ITest test6;

	private string myString;
	private string myString2;

	[Inject("aname")]
	public ITest test;

	[Inject]
	public ITest test4;

	public int num;

	[Inject]
	public TestClass Test2 { get; set; }

	[Inject("another.string")]
	public string aString { get; set;}

	[Inject]
	public float FloatTest { get; set; }

	[Inject("inject.inject")]
	public string instantiateTest;

	[Inject]
	public TestSignal signal;

	[Inject]
	public ExceptionSignal exceptionSignal;

	[Inject]
	public ObjectInjectImpl(TestClass testClass, [Named("aname")] ITest test5, ITest test6,[Named("string.name")] string aString,
	                        [Named("another.string")] string bString, int aNumber) {
		this.test3 = testClass;
		this.test5 = test5;
		this.test6 = test6;
		this.myString = aString;
		this.myString2 = bString;
		this.num = aNumber;
	}

	public void PrintAFew() {
		Assert(test.Print().Equals("ANOTHER PRINT"), "Test wasn't equal to \"ANOTHER PRINT\"");
		Test2.PrintName();
		test3.PrintName();
		test4.Print();
		test5.Print();
		test6.Print();


		Debug.Log (myString);
		Debug.Log (myString2);
		Debug.Log (this.num);
		Debug.Log (FloatTest);
		Debug.Log (aString);

		test3.ChangeName("CHANGED");

		signal.Dispatch();
	}

	public void ChangeName(string aname) {
		Test2.ChangeName(aname);
	}

	private void Assert(bool condition, string message) {
		if (!condition) {
			exceptionSignal.Dispatch(message);
			throw new System.Exception(message);
		}
	}
}
