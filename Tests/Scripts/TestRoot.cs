using UnityEngine;
using System.Collections;

using Mestevens.Injection.Core;

public class TestRoot : MonoBehaviour {
	
	string output = "";

	// Use this for initialization
	void Start () {
		Application.RegisterLogCallback(HandleLog);
		Context context = Miranda.Init(new ParentContext(), new OtherContext());

		ExceptionSignal eS = context.Get<ExceptionSignal>();
		eS.AddOnce((x) => {
			Debug.Log (x[0]);
		});

		IObjectInject tA = context.Get<IObjectInject>();
		tA.PrintAFew();

		Debug.Log ("+++++++++++++++++++++++++");

		IObjectInject tAA = context.Get<IObjectInject>();
		tAA.PrintAFew();

		Debug.Log ("+++++++++++++++++++++++++");

		tAA.ChangeName("TESTINGOFSINGLETON");

		tA.PrintAFew();

		TestSignal testSignal = context.Get<TestSignal>();
		testSignal.AddOnce((x) => {
			foreach(object o in x) {
				Debug.Log (o);
			}
		});

		testSignal.Dispatch("A", "b", "C");

		TestSignal anotherSignal = context.Get<TestSignal>();
		anotherSignal.Dispatch();
	}
	
	void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}
	
	void HandleLog(string logString, string stackTrace, LogType type)
	{
		output += logString + "\n";
	}

	public Vector2 scrollPosition;

	void OnGUI () {

		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
		GUILayout.Label(output);
		if (GUILayout.Button("Clear"))
			output = "";
		
		GUILayout.EndScrollView();

	}

}
