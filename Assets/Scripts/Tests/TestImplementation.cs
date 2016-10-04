using Miranda;

public class TestImplementation : ITestInterface {

	private readonly string text;

	[Inject]
	public TestImplementation([Named("test")] string text) {
		this.text = text;
	}

	public void Output() {
		UnityEngine.Debug.Log("Output " + text);
	}
}
