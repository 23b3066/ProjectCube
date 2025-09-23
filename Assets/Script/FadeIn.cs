using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public Image panelImage;             // フェード対象のImage（Canvasに設置）
    public float fadeDuration = 2.0f;    // フェード時間（秒）
    public float delayBeforeFade = 0.1f; // フェード開始までの待ち時間（秒）

    void Start()
    {
        // 最初に完全に黒にしておく（念のため）
        if (panelImage != null)
        {
            Color c = panelImage.color;
            c.a = 1f;
            panelImage.color = c;

            StartCoroutine(FadeOutPanel());
        }
    }

    IEnumerator FadeOutPanel()
    {
        yield return new WaitForSeconds(delayBeforeFade); // SceneFaderの後に開始

        float elapsed = 0f;
        Color color = panelImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration); // 徐々に透明に
            color.a = alpha;
            panelImage.color = color;
            yield return null;
        }

        // 誤差を防ぐために完全に透明に
        color.a = 0f;
        panelImage.color = color;

        // 必要なら Image を無効化（クリックブロック回避）
        panelImage.raycastTarget = false;
    }
}
