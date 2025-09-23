using UnityEngine;

public class KeyBoard_test : MonoBehaviour
{
    int selectCubeFace;

    public GimmickController_test gimmickController;

    void Start() 
    {
        selectCubeFace = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            selectCubeFace = 0;
            Debug.Log("d が押された");
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            Debug.Log("w が押された");
            selectCubeFace = 1;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log("a が押された");
            selectCubeFace = 2;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log("s が押された");
            selectCubeFace = 3;
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            gimmickController.keyReceived(24 + selectCubeFace*3);
            Debug.Log("テンキー 1 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            gimmickController.keyReceived(25 + selectCubeFace*3);
            Debug.Log("テンキー 2 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            gimmickController.keyReceived(26 + selectCubeFace*3);
            Debug.Log("テンキー 3 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            gimmickController.keyReceived(12 + selectCubeFace*3);
            Debug.Log("テンキー 4 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            gimmickController.keyReceived(13 + selectCubeFace*3);
            Debug.Log("テンキー 5 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            gimmickController.keyReceived(14 + selectCubeFace*3);
            Debug.Log("テンキー 6 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            gimmickController.keyReceived(0 + selectCubeFace*3);
            Debug.Log("テンキー 7 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            gimmickController.keyReceived(1 + selectCubeFace*3);
            Debug.Log("テンキー 8 が押された");
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            gimmickController.keyReceived(2 + selectCubeFace*3);
            Debug.Log("テンキー 9 が押された");
        }
    }
}
