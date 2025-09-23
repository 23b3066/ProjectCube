using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class ControlleVFX : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    [SerializeField] GameObject effect_tile;
    public Transform effect_tile_position;

    static string keyName = "StopPlay";
    private Coroutine fadeCoroutine;
    private Coroutine restoreCoroutine;
    public float downFilter = 0.001f;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        effect.SendEvent(keyName);
        effect_tile.SetActive(false);

        originalPosition = effect_tile.transform.position;
        originalScale = effect_tile.transform.localScale;
    }

    public void OnHoverEnter()
    {
        if (keyName != "OnPlay")
        {
            keyName = "OnPlay";
            effect.SendEvent(keyName);
            Debug.Log(keyName);

            // フェードアウト中なら停止
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            // 元に戻すコルーチンが動いてたら止める（念のため）
            if (restoreCoroutine != null)
            {
                StopCoroutine(restoreCoroutine);
                restoreCoroutine = null;
            }

            effect_tile.SetActive(true);

            // 現在の位置・スケールから徐々に元の状態に戻す
            restoreCoroutine = StartCoroutine(RestorePositionAndScale(0.5f)); // 0.5秒で戻す
        }
    }

    public void OnHoverExit()
    {
        if (keyName != "StopPlay")
        {
            keyName = "StopPlay";
            effect.SendEvent(keyName);
            Debug.Log(keyName);

            if (restoreCoroutine != null)
            {
                StopCoroutine(restoreCoroutine);
                restoreCoroutine = null;
            }

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutAndDisable(1f));
        }
    }

    private IEnumerator FadeOutAndDisable(float duration)
    {
        float time = 0f;

        Vector3 startPos = effect_tile.transform.position;
        Vector3 targetPos = startPos + Vector3.down * 1f * downFilter;

        Vector3 startScale = effect_tile.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            effect_tile.transform.position = Vector3.Lerp(startPos, targetPos, t);
            effect_tile.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        effect_tile.transform.position = targetPos;
        effect_tile.transform.localScale = targetScale;

        effect_tile.SetActive(false);

        fadeCoroutine = null;
        Debug.Log("Effect tile faded out and disabled");
    }

    private IEnumerator RestorePositionAndScale(float duration)
    {
        float time = 0f;

        Vector3 startPos = effect_tile.transform.position;
        Vector3 targetPos = originalPosition;

        Vector3 startScale = effect_tile.transform.localScale;
        Vector3 targetScale = originalScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            effect_tile.transform.position = Vector3.Lerp(startPos, targetPos, t);
            effect_tile.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        effect_tile.transform.position = targetPos;
        effect_tile.transform.localScale = targetScale;

        restoreCoroutine = null;
        Debug.Log("Effect tile restored to original position and scale");
    }
}
