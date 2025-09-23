using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Gostart : MonoBehaviour
{
    public float Delay = 5f; // シーン切り替えまでの遅延（秒）
    public AudioSource bgmAudioSource;

    // 最初に一度だけ実行される
    void Start()
    {

        // コルーチンを開始する
        StartCoroutine(WaitAndLoadScene());
        bgmAudioSource.Play();
    }

    // 遅延してからシーンを切り替える処理
    IEnumerator WaitAndLoadScene()
    {
        // 指定した秒数待機
        yield return new WaitForSeconds(Delay);

        // "Start" シーンに切り替える
        SceneManager.LoadScene("Start");
    }
}
