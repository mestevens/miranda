using UnityEngine;
using Miranda;

public class SubBehaviour : MonoBehaviour {

	[Inject]
	public ITestInterface testInterface { get; set; }

	// Use this for initialization
	void Start () {
		testInterface.Output();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
