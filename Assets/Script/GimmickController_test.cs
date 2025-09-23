using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI用

public class GimmickController_test : MonoBehaviour
{
    public int FloorCount = 0;
    public ParticleSystem[] particles;  // 風用パーティクル36個
    [SerializeField] public GameObject biribir;  // びりびり用

    //<==============パーティクル(風)===============>
    private bool[] particleAble;  
    private float[] particleTime;  
    public float WindDelay = 2.5f;

    //<==============パーティクル(びりびり)===============>
    private bool[] biribirAble;  
    private float[] biribirTime;  
    public float wallSpeed = -1f;  
    public float wallDstyTime = 2f;  
    public float WallDelay = 3f;
    public Transform[] biribirPositions; // 36個をシーン上に置く

    //<==============ロボット(3層目)===============>
    private bool[] robotAble;
    private float[] robotTime;
    public float RobotDelay = 4f; // 個別ディレイ
    public Transform[] robotPositions; // 36個をシーン上に置く
    public GameObject[] robots;  // 出現させたいロボットのPrefab配列

    // カメラ遷移
    public Transform camera;
    public Transform secondFloor;
    public Transform thirdFloor;
    private int floor = 1;

    // ========= EndUI関連 =========
    public GameObject endUIPanel; // 終了時に表示するUI
    private CanvasGroup endUIGroup;

    // === SerialHandler 参照はコードで取得 ===
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

        //<=========風============>
        particleAble = new bool[particles.Length];
        particleTime = new float[particles.Length];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop();
            particleAble[i] = false;
        }

        //<==========びりびり==============>
        biribirAble = new bool[36];
        biribirTime = new float[36];
        for (int i = 0; i < 36; i++)
        {
            biribirAble[i] = false;
        }

        //<==========ロボット==============>
        robotAble = new bool[36];
        robotTime = new float[36];
        for (int i = 0; i < 36; i++)
        {
            robotAble[i] = false;
        }

        //<==========EndUI==============>
        if (endUIPanel != null)
        {
            endUIGroup = endUIPanel.GetComponent<CanvasGroup>();
            if (endUIGroup == null)
            {
                endUIGroup = endUIPanel.AddComponent<CanvasGroup>();
            }
            endUIGroup.alpha = 0;
            endUIPanel.SetActive(false);
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

        //<========= Floor1: 風 ===========>
        if(FloorCount == 0) 
        {
            for (int i = 0; i < particles.Length; i++)
            {
                if (particleAble[i] && currentTime - particleTime[i] >= WindDelay)
                {
                    particleAble[i] = false;
                    particles[i].Stop();
                }
            }
        }
        //<========= Floor2: びりびり ===========>
        else if(FloorCount == 1) 
        {
            for (int i = 0; i < biribirAble.Length; i++)
            {
                if (biribirAble[i] && currentTime - biribirTime[i] >= WallDelay)
                {
                    biribirAble[i] = false;
                }
            }
            if(floor == 1)
            {
                camera.position = secondFloor.position;
                floor += 1;
            }
        }
        //<========= Floor3: ロボット ===========>
        else if(FloorCount == 2) 
        {
            for (int i = 0; i < robotAble.Length; i++)
            {
                if (robotAble[i] && currentTime - robotTime[i] >= RobotDelay)
                {
                    robotAble[i] = false;
                }
            }
            if(floor == 2) 
            {
                camera.position = thirdFloor.position;
                floor += 1;
            }   
        }
        //<========= End ===========>
        else if(FloorCount == 3)
        {
            SceneManager.LoadScene("GameClear");
        }
    }

    void OnDataReceived(string message)
    {
        Debug.Log(message);
        string str_num = message.Substring("Pressed:".Length).Trim();
        if(int.TryParse(str_num, out int num)) 
        {
            if(num < 0 || num >= 36) 
            {
                Debug.LogWarning($"範囲外のデータが来ました: {num}");
                return;
            }

            //<========== Floor1: 風 ==========>
            if (FloorCount == 0)
            {
                if (!particleAble[num]) 
                {
                    particles[num].Play();
                    particleAble[num] = true;
                    particleTime[num] = Time.time;
                }
            }
            //<========== Floor2: びりびり ==========>
            else if(FloorCount == 1)
            {
                if (!biribirAble[num]) 
                {
                    biribirAble[num] = true;
                    biribirTime[num] = Time.time;
                    SpawnBiribir(num);
                }
            }
            //<========== Floor3: ロボット ==========>
            else if(FloorCount == 2)
            {
                if (!robotAble[num]) 
                {
                    robotAble[num] = true;
                    robotTime[num] = Time.time;
                    SpawnRobot(num);
                }
            }
        }
        else
        {
            Debug.LogWarning($"送信データが正しく送られていません: {message}");
        }
    }

    //キーボードテスト用関数
    public void keyReceived(int num)
    {
        OnDataReceived($"Pressed:{num}");
    }

    //<===============びりびり関連===============>
    private void SpawnBiribir(int index)
    {
        if(biribirPositions == null || index < 0 || index >= biribirPositions.Length)
        {
            Debug.LogWarning($"Spawn位置が設定されていません: {index}");
            return;
        }

        Vector3 position = biribirPositions[index].position;
        GameObject obj = Instantiate(biribir, position, Quaternion.identity);
        StartCoroutine(MoveAndDestroy(obj,index));
    }

    private IEnumerator MoveAndDestroy(GameObject obj, int index) 
    {
        float timer = 0f;
        while (timer < wallDstyTime)
        {
            switch(index % 12) 
            {
                case 0:
                case 1:
                case 2:
                    obj.transform.Translate(new Vector3(wallSpeed, 0, 0) * Time.deltaTime, Space.World);
                    break;
                case 3:
                case 4:
                case 5:
                    obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    obj.transform.Translate(new Vector3(0, 0, wallSpeed) * Time.deltaTime, Space.World);
                    break;
                case 6:
                case 7:
                case 8:
                    obj.transform.Translate(new Vector3(-wallSpeed, 0, 0) * Time.deltaTime, Space.World);
                    break;
                case 9:
                case 10:
                case 11:
                    obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    obj.transform.Translate(new Vector3(0, 0, -wallSpeed) * Time.deltaTime, Space.World);
                    break;
            }
            
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    //<===============ロボット関連===============>
    private void SpawnRobot(int index)
    {
        if(robotPositions == null || index < 0 || index >= robotPositions.Length)
        {
            Debug.LogWarning($"ロボットSpawn位置が設定されていません: {index}");
            return;
        }
        if(robots == null || robots.Length == 0)
        {
            Debug.LogWarning("ロボットPrefabが設定されていません");
            return;
        }

        int rand = Random.Range(0, robots.Length);
        Vector3 position = robotPositions[index].position;
        Instantiate(robots[rand], position, Quaternion.identity);
    }
}
