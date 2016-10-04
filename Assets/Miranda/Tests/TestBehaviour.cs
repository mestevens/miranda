using UnityEngine;

using Miranda;

public class TestBehaviour : MonoBehaviour {

	public string test { get; set; }

	public int number { get; set; }

	[Inject("test")]
	public string anotherTest { get; set; }

	[Inject]
	public TestService TestService { get; set; }

	[Inject]
	public TestService AnotherService { get; set; }

	[Inject]
	public TestNoInject TestNoInject { get; set; }

	// Use this for initialization
	void Start () {
		Injection.CreateStaticInjector(new TestModule());
		Injection.InjectIntoScene();

		Debug.Log(test);
		Debug.Log(number);
		TestService.WriteAString();
		TestService.WriteAString();
		AnotherService.WriteAString();
		AnotherService.WriteAString();
		Debug.Log(anotherTest);
		TestNoInject.printMe();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Inject]
	public void testMethodInjection(string str, int num) {
		this.test = str;
		this.number = num;
	}
}
