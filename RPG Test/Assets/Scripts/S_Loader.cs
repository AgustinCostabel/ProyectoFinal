using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class S_Loader {
    public enum Scene {
        MainMenu,
        Game,
        Loading
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene) {
        S_Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
