using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// シーン遷移時に画面とBGMをフェードアウト・フェードインさせるクラス
/// </summary>
public class SceneFader : MonoBehaviour
{
    public Image fadeImage;                  // 黒いフェード用イメージ（Canvas 上に配置）
    public float fadeDuration = 1f;          // フェード時間（秒）
    public string debugSceneName = "stage1"; // デバッグ用シーン名

    public AudioSource bgmAudioSource;       // フェードさせるBGMのAudioSource

    private bool isFading = false;           // フェード中かどうか

    public string playerTag = "Player";           // プレイヤーのタグ名

    private void Awake()
    {
        // シーンが切り替わってもこの GameObject を破棄しない
        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // デバッグ用：Shiftキーでシーン切り替え
        // if (!isFading && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        // {
        //     Debug.Log("Shiftキーが押されました。シーンを遷移します。");
        //     FadeAndLoadScene(debugSceneName);
        // }
    }

    /// <summary>
    /// 指定されたシーンにフェード付きで遷移する
    /// </summary>
    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag(playerTag))
    {
        FadeAndLoadScene(debugSceneName);
    }
}


    /// <summary>
    /// フェード → シーン読み込み → フェード解除の流れを制御
    /// </summary>
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        isFading = true;

        // BGMフェードアウトも同時に開始
        if (bgmAudioSource != null)
        {
            StartCoroutine(FadeOutAudio(bgmAudioSource, fadeDuration));
        }

        // 画面フェードアウト
        yield return StartCoroutine(Fade(1f, fadeDuration));

        // シーンの非同期読み込み
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame(); // 読み込み直後に1フレーム待機
        yield return new WaitForSeconds(0.1f); // 少し余裕を持たせる

        // // 少し待機して演出
        // yield return new WaitForSeconds(0.2f);

        // フェードイン（ゆっくり）
        yield return StartCoroutine(Fade(0f, fadeDuration * 2f));

        isFading = false;
    }

    /// <summary>
    /// AudioSourceの音量を徐々に下げてフェードアウト
    /// </summary>
    private IEnumerator FadeOutAudio(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audio.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        audio.volume = 0f;
        audio.Stop();
    }

    /// <summary>
    /// UI イメージのアルファを変えてフェードする処理
    /// </summary>
    private IEnumerator Fade(float targetAlpha, float duration)
    {
        fadeImage.raycastTarget = true; // UI操作ブロック
        float startAlpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 最終的な透明度をセット
        fadeImage.color = new Color(0, 0, 0, targetAlpha);
        fadeImage.raycastTarget = (targetAlpha != 0f); // 完全透明なら操作許可
    }
}
