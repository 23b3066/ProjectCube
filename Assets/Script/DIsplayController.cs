using UnityEngine;

public class DisplayController : MonoBehaviour
{

    [SerializeField, Range( 1, 8)]

    private int m_useDisplayCount = 2;

    private void Awake() {
        int count = Mathf.Min(Display.displays.Length, m_useDisplayCount);
        for(int i=0; i < count; ++i) {
            Display.displays[i].Activate();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
