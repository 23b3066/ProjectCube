using UnityEngine;

public class ButtonMatrixController : MonoBehaviour
{
    [SerializeField] private SerialHandler serialHandler;

    void OnEnable()
    {
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void OnDisable()
    {
        serialHandler.OnDataReceived -= OnDataReceived;
    }

    private void OnDataReceived(string msg)
    {
        Debug.Log(msg);
        string numPart = msg.Substring("Pressed:".Length).Trim();
        if (int.TryParse(numPart, out int idx))
        {
            Highlight(idx);
        }
    }

    private void Highlight(int index)
    {
        int row = index / 3;
        int col = index % 3;
        Debug.Log($"ハイライト: row {row}, col {col} ");
    }
}
