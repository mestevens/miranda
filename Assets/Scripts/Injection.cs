using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miranda {

public class Injection {

	private static Injector injector;

	public static Injector CreateStaticInjector(params Module[] modules) {
		injector = new Injector();
		foreach (Module module in modules) {
			module.Configure(injector);
		}
		return injector;
	}

	public static Injector CreateInjector(params Module[] modules) {
		Injector localInjector = new Injector();
		foreach (Module module in modules) {
			module.Configure(localInjector);
		}
		return localInjector;
	}

	public static void InjectIntoScene() {
		if (injector != null) {
			Scene scene = SceneManager.GetActiveScene();
			GameObject[] gameObjects = scene.GetRootGameObjects();
			foreach (GameObject gameObject in gameObjects) {
				Component[] components = gameObject.GetComponents<Component>();
				foreach (Component component in components) {
					injector.InjectIntoObject(component);
				}
			}
		}
	}

}

}
