using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;

public class SerialHandler : MonoBehaviour
{
    public static SerialHandler Instance { get; private set; }

    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    public string portName = "COM4";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    private CancellationTokenSource cancelSrc_;
    private bool isNewMessageReceived_ = false;
    private string message_;
    private readonly object lockObject_ = new object();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // シーンを跨いでも残す
        Open();
    }

    void Update()
    {
        lock (lockObject_)
        {
            if (isNewMessageReceived_)
            {
                OnDataReceived?.Invoke(message_);
                isNewMessageReceived_ = false;
            }
        }
    }

    private void OnApplicationQuit() => Close();

    private void Open()
    {
        try
        {
            serialPort_ = new SerialPort(portName, baudRate)
            {
                ReadTimeout = 100,
                WriteTimeout = 100,
                DtrEnable = true
            };
            serialPort_.Open();

            cancelSrc_ = new CancellationTokenSource();
            thread_ = new Thread(() => Read(cancelSrc_.Token)) { IsBackground = true };
            thread_.Start();

            Debug.Log($"Serial opened on {portName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open {portName}: {e.Message}");
        }
    }

    private void Close()
    {
        cancelSrc_?.Cancel();
        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join(500); // Abort は使わない
        }
        if (serialPort_ != null)
        {
            if (serialPort_.IsOpen) serialPort_.Close();
            serialPort_.Dispose();
            serialPort_ = null;
        }
        Debug.Log("Serial closed");
    }

    private void Read(CancellationToken token)
    {
        while (!token.IsCancellationRequested && serialPort_?.IsOpen == true)
        {
            try
            {
                string raw = serialPort_.ReadLine();
                string line = raw.Trim();
                lock (lockObject_)
                {
                    message_ = line;
                    isNewMessageReceived_ = true;
                }
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                Debug.LogError($"Read Error: {e.Message}");
                break;
            }
        }
    }

    public void Write(string message)
    {
        try { serialPort_?.WriteLine(message); }
        catch (Exception e) { Debug.LogWarning($"Write Error: {e.Message}"); }
    }

    public bool IsConnected() => serialPort_?.IsOpen == true;
}
