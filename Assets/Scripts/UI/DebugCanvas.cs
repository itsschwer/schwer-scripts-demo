using System.Collections;
using Schwer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviourSingleton<DebugCanvas> {
    [Header("Quit display")]
    [SerializeField] private Text quitLog = default;
    [SerializeField] private float quitHoldDuration = 5;
    [Header("Save display")]
    [SerializeField] private Text saveLog = default;
    [SerializeField] private float displayDuration = 1;
    [SerializeField] private float fadeDuration = 0.5f;
    private Coroutine runningCoroutine;

    private float _timer;
    private float timer {
        get => _timer;
        set => _timer = (value < 0) ? 0 : value;
    }

    protected override void Awake() {
        base.Awake();
        quitLog.text = saveLog.text = "";
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        if (Application.platform != RuntimePlatform.WebGLPlayer) {
            if (Input.GetButton("Cancel")) {
                quitLog.canvasRenderer.SetAlpha(1); // Need to set to non-zero, since fading in from zero seems buggy
                timer += Time.unscaledDeltaTime;
                if (timer > quitHoldDuration) {
                    Application.Quit();
                    // Debug.Log("Quit");
                }
            }
            else if (timer > 0) {
                timer -= Time.unscaledDeltaTime;
            }
            quitLog.text = "Quitting... (" + Mathf.RoundToInt(quitHoldDuration - timer + 0.5f) + ")";
            quitLog.canvasRenderer.SetAlpha(timer / quitHoldDuration);
        }
    }

    public void Display(string message) {
        if (runningCoroutine != null) {
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        saveLog.canvasRenderer.SetAlpha(1);
        saveLog.text = message;

        runningCoroutine = StartCoroutine(FadeTextCo());
    }

    private IEnumerator FadeTextCo() {
        yield return new WaitForSecondsRealtime(displayDuration);
        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime) {
            saveLog.canvasRenderer.SetAlpha(1 - (t / fadeDuration));
            yield return null;
        }
        runningCoroutine = null;
    }
}
