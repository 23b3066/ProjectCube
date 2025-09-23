using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveVR : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Jump = 5f; //JUMPPOWER
    private Rigidbody rb;    
    //Vector3 pos = transform.position;
    public GameObject Player;
    public GameObject view;
    [SerializeField] float SpeadScale;

        void Start()
    {
        transform.position = new Vector3(0, 0f ,0);
        rb = Player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 左手のアナログスティックの向きを取得
        // Vector2 stickL = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        // 右手のアナログスティックの向きを取得

        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        // Debug.Log("stickL:"+ stickL);
        // Debug.Log("stickR:"+ stickR);
        Vector3 changePosition = new Vector3((stickR.x)/SpeadScale, 0f, (stickR.y)/SpeadScale);

        Vector3 vision = view.transform.rotation.eulerAngles;

        Player.transform.position += Player.transform.rotation * (Quaternion.Euler(vision) * changePosition);

        // Player.transform.rotation = Quaternion.Euler(vision);
        // Vector3 currentRotation = Player.transform.rotation.eulerAngles;
        // Player.transform.rotation = Quaternion.Euler(vision.x,vision.y,vision.z);
        // Debug.Log("stickRx:"+ stickR.x);
        // Debug.Log("stickRy:"+ stickR.y);
        // Player.transform.Translate((stickR.x)/SpeadScale,0,(stickR.y)/SpeadScale,Space.World); 
        if (OVRInput.GetDown(OVRInput.Button.Three)) // 右手コントローラーのXボタンが押された
    {
        if (rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.01f) // 地面にいる場合
        {
            rb.AddForce(Vector3.up * Jump, ForceMode.Impulse);
        }
    } 
    }
}
