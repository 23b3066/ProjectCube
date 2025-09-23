using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GimmickController : MonoBehaviour
{
    public int FloorCount = 0;
    public Light[] lights;
    public ParticleSystem[] particles;
    [SerializeField] public GameObject biribir;
    [SerializeField] public CanvasGroup[] warning;
    public float fadeDuration = 1f;
    private Coroutine currentFade;

    //<================ライト====================>
    private bool[] lightDis;
    private float[] lightTime;
    public float LightDelay = 1.5f;

    //<==============パーティクル(風)===============>
    private bool[] particleAble;
    private float[] particleTime;
    public float WindDelay = 2.5f;

    //<==============パーティクル(壁)===============>
    private bool[] wallAble;
    private float[] wallTime;
    public Transform right_tile;
    public Transform center_tile;
    public Transform left_tile;
    public float wallSpeed = -1f;
    public float wallDstyTime = 2f;
    public float WallDelay = 3f;

    private Vector3 right_pos, center_pos, left_pos;

    // カメラ遷移
    public Transform camera;
    public Transform secondFloor;
    public Transform thirdFloor;
    private int floor = 1;

    // === キーボード用設定 ===
    [Header("ギミック番号(0,1,2 / 12,13,14 / 24,25,26)に対応するキー設定")]
    public KeyCode[] keyBindings = new KeyCode[9]
    {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2,
        KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8
    };

    private SerialHandler serialHandler;

    void Start()
    {
        // シングルトンから取得
        serialHandler = SerialHandler.Instance;
        if (serialHandler != null)
        {
            serialHandler.OnDataReceived -= OnDataReceived; // 二重登録防止
            serialHandler.OnDataReceived += OnDataReceived;
        }

        // ライト初期化
        lightDis = new bool[lights.Length];
        lightTime = new float[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
            lightDis[i] = false;
        }

        // パーティクル初期化
        particleAble = new bool[particles.Length];
        particleTime = new float[particles.Length];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
            particleAble[i] = false;
        }

        // 壁初期化
        right_pos = right_tile.position;
        center_pos = center_tile.position;
        left_pos = left_tile.position;
        wallAble = new bool[3];
        wallTime = new float[3];
        for (int i = 0; i < wallAble.Length; i++)
        {
            wallAble[i] = false;
        }
    }

    void OnDestroy()
    {
        if (serialHandler != null)
        {
            serialHandler.OnDataReceived -= OnDataReceived;
        }
    }

    void Update()
    {
        float currentTime = Time.time;

        // === Floor別の処理 ===
        if (FloorCount == 0)
        {
            for (int i = 0; i < lights.Length; i++)
            {
                if (lightDis[i] && currentTime - lightTime[i] >= LightDelay)
                {
                    lightDis[i] = false;
                    lights[i].enabled = true;
                }
            }
        }
        else if (FloorCount == 1)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particleAble[i] && currentTime - particleTime[i] >= WindDelay)
                {
                    StartCoroutine(FadeOut(warning[i], fadeDuration));
                    particleAble[i] = false;
                    particles[i].Stop();
                }
            }
            if (floor == 1)
            {
                camera.position = secondFloor.position;
                floor += 1;
            }
        }
        else if (FloorCount == 2)
        {
            for (int i = 0; i < wallAble.Length; i++)
            {
                if (wallAble[i] && currentTime - wallTime[i] >= WallDelay)
                {
                    StartCoroutine(FadeOut(warning[i + 3], fadeDuration));
                    wallAble[i] = false;
                }
            }
            if (floor == 2)
            {
                camera.position = thirdFloor.position;
                floor += 1;
            }
        }
        else if (FloorCount == 3)
        {
            SceneManager.LoadScene("GameClear");
        }

        // === キーボード入力チェック ===
        for (int i = 0; i < keyBindings.Length; i++)
        {
            if (Input.GetKeyDown(keyBindings[i]))
            {
                int[] validNums = { 0, 1, 2, 12, 13, 14, 24, 25, 26 };
                HandleInput(validNums[i]);
            }
        }
    }

    // シリアル受信時
    void OnDataReceived(string message)
    {
        string str_num = message.Substring("Pressed:".Length).Trim();
        if (int.TryParse(str_num, out int num))
        {
            HandleInput(num);
        }
        else
        {
            Debug.LogWarning($"送信データが正しく送られていません: {message}");
        }
    }

    private void HandleInput(int num)
    {
        int col = -1;
        if (num == 0 || num == 12 || num == 24) col = 0;
        else if (num == 1 || num == 13 || num == 25) col = 1;
        else if (num == 2 || num == 14 || num == 26) col = 2;
        else
        {
            Debug.LogWarning($"未対応の入力: num={num}");
            return;
        }

        if (FloorCount == 0 && !lightDis[col])
        {
            for (int i = 0; i < 3; i++)
            {
                int index = col + (3 * i);
                lights[index].enabled = false;
                lightDis[index] = true;
                lightTime[index] = Time.time;
            }
        }
        else if (FloorCount == 1 && !particleAble[col])
        {
            StartCoroutine(FadeIn(warning[col], fadeDuration));
            particles[col].Play();
            particleAble[col] = true;
            particleTime[col] = Time.time;
        }
        else if (FloorCount == 2 && !wallAble[col])
        {
            StartCoroutine(FadeIn(warning[col + 3], fadeDuration));
            wallAble[col] = true;
            wallTime[col] = Time.time;

            if (col == 0) SpawnBiribir(left_pos);
            if (col == 1) SpawnBiribir(center_pos);
            if (col == 2) SpawnBiribir(right_pos);
        }
    }

    private void SpawnBiribir(Vector3 position)
    {
        float p = 10f;
        if (Random.Range(1, 3) == 1) position.y -= p;
        else position.y -= 3f;

        GameObject obj = Instantiate(biribir, position, Quaternion.identity);
        StartCoroutine(MoveAndDestroy(obj));
    }

    private IEnumerator MoveAndDestroy(GameObject obj)
    {
        float timer = 0f;
        while (timer < wallDstyTime)
        {
            obj.transform.Translate(new Vector3(wallSpeed, 0, 0));
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
