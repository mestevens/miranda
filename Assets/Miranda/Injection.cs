using UnityEngine;
using UnityEngine.SceneManagement;

namespace Miranda {

public class Injection {

	public static Injector injector;

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
			for (int i = 0; i < SceneManager.sceneCount; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				GameObject[] gameObjects = scene.GetRootGameObjects();
				foreach (GameObject gameObject in gameObjects) {
					InjectIntoGameObject(gameObject);
				}
			}
		}
	}

	private static void InjectIntoGameObject(GameObject gameObject) {
		Component[] components = gameObject.GetComponents<Component>();
		foreach (Component component in components) {
			injector.InjectIntoObject(component);
		}
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			GameObject child = gameObject.transform.GetChild(i).gameObject;
			InjectIntoGameObject(child);
		}
	}

}

}
