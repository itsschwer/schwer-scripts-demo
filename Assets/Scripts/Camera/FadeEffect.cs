using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FadeEffect : MonoBehaviour {
    [SerializeField] private Color color = Color.black;
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private Shader shader = default;
    private Material material;

    private void Awake() {
        if (shader != null) {
            material = new Material(shader);
            material.SetColor("_Color", color);
            StartCoroutine(FadeInCo());
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (material == null) {
            Graphics.Blit(src, dest);
        }
        else {
            Graphics.Blit(src, dest, material);
        }
    }

    private IEnumerator FadeInCo() {
        for (float t = fadeInDuration; t > 0; t -= Time.unscaledDeltaTime) {
            Shader.SetGlobalFloat("_NormalisedProgress", t / fadeInDuration);
            yield return null;
        }
        Shader.SetGlobalFloat("_NormalisedProgress", 0);
    }
}
