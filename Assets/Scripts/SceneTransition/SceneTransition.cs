using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using USM = UnityEngine.SceneManagement.SceneManager;

public static class SceneTransition {
    public static Action<Vector2, Vector2> OnNewSceneOrient;

    private static Vector2? nextScenePosition;
    private static Vector2? nextSceneDirection;

    public static void SetNextSceneOrientation(Vector2 position, Vector2 direction) {
        nextScenePosition = position;
        nextSceneDirection = direction;
    }

    private static void OrientNewScene(Scene scene, LoadSceneMode loadSceneMode) {
        if (nextScenePosition != null && nextSceneDirection != null) {
            OnNewSceneOrient?.Invoke(nextScenePosition.Value, nextSceneDirection.Value);
        }

        USM.sceneLoaded -= OrientNewScene;
    }

    public static IEnumerator LoadSceneAsyncCo(string sceneName, float minDuration) {
        USM.sceneLoaded += OrientNewScene;

        var asyncOp = USM.LoadSceneAsync(sceneName);

        if (minDuration > 0) {
            asyncOp.allowSceneActivation = false;
            yield return new WaitForSecondsRealtime(minDuration);
            for (float t = 0; t < minDuration; t += Time.unscaledDeltaTime) {
                Shader.SetGlobalFloat("_NormalisedProgress", t / minDuration);
                yield return null;
            }
            Shader.SetGlobalFloat("_NormalisedProgress", 1);
            asyncOp.allowSceneActivation = true;
        }

        while (!asyncOp.isDone) {
            yield return null;
        }
    }
}
