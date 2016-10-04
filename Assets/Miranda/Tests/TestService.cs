using Miranda;

public class TestService {

	private string Text;
	private int number;

	[Inject]
	public TestService([Named("test")] string text) {
		this.Text = text;
		this.number = 10;
	}

	[Inject]
	public void MethodInjection([Named("test")] int num) {
		this.number = num;
	}

	public void WriteAString() {
		UnityEngine.Debug.Log("Write this string: " + Text);
		UnityEngine.Debug.Log(this.number);
		Text = "ANOTHER TEXT";
	}
}
