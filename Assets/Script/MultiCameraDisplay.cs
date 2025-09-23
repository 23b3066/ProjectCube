using UnityEngine;
using UnityEngine.XR;

public class MultiCameraDisplay : MonoBehaviour
{
    Camera MainCamera;
    
    void Start()
    {
        //カメラコンポーネントを取得
        MainCamera = GetComponent<Camera>();
        //PCディスプレイ表示を三人称視点カメラに切り替え
        OnThirdView();
    }
    
    //PCディスプレイにプレイヤー目線を表示
    void OnPlayerView()
    {
        MainCamera.enabled = false;
        XRSettings.gameViewRenderMode = GameViewRenderMode.LeftEye;
    }
    
    //PCディスプレイにThirdViewCamera映像を表示
    void OnThirdView()
    {
        MainCamera.enabled = true;
        XRSettings.gameViewRenderMode = GameViewRenderMode.None;
    }
}
