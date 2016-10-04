public class TestNoInject {

	private string text;

	public TestNoInject() {
		text = "asdf";
	}

	public void printMe() {
		UnityEngine.Debug.Log("Print Me: " + text);
	}
	
}
