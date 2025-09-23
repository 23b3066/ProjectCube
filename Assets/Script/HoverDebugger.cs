using UnityEngine;

public class HoverDebugger : MonoBehaviour
{
    public void OnHoverEnter()
    {
        Debug.Log($"{gameObject.name} にホバーされました");
    }

    public void OnHoverExit()
    {
        Debug.Log($"{gameObject.name} からホバーが外れました");
    }
}
