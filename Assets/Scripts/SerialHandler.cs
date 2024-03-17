using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;


public class SerialHandler : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    public bool IsRunning_ { get; set; } = false;
    

    // Thread safe queue
    public ConcurrentQueue<byte[]> cmds = new();
    //Action ISerialHandler.OnPortOpened { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private string message_;
    private volatile bool isNewMessageReceived_ = false;

    public delegate void PortOpenedHandler();
    public event PortOpenedHandler OnPortOpened;

    byte[] buffer = new byte[100];

    void Start()
    {
        Screen.fullScreen = true;
        LoadSerialSettings();
        OpenPortWithNewName(portName);
    }

    public void LoadSerialSettings()
    {
        string path = Path.Combine(Application.dataPath, "../Settings/serial_settings.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SerialSettings settings = JsonUtility.FromJson<SerialSettings>(json);
            portName = settings.portName;
            baudRate = settings.baudRate;
            OpenPortWithNewName(portName);
        }
        else
        {
            Debug.LogError("Not find settings file");
        }
    }

    void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        try
        {
            serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            serialPort_.Open();
            serialPort_.ReadTimeout = 50;

            IsRunning_ = true;

            thread_ = new Thread(Read);
            thread_.Start();

            // Notification port is open
            OnPortOpened?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to open serial port {portName}: {e.Message}");
        }
    }

    private void Close()
    {
        isNewMessageReceived_ = false;
        IsRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join();
        }

        if (serialPort_ != null)
        {
            try
            {
                if (serialPort_.IsOpen)
                {
                    serialPort_.Close();
                }
            }
            finally
            {
                serialPort_.Dispose();
            }
        }

    }

    public void OpenPortWithNewName(string newPortname)
    {
        if (serialPort_ != null && serialPort_.IsOpen)
        {
            Close();
        }

        portName = newPortname;

        Open();
    }

    private void Read()
    {
        while (IsRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                if (serialPort_.BytesToRead > 0)
                {
                    Array.Clear(buffer, 0, buffer.Length);

                    int bytesRead = serialPort_.Read(buffer, 0, buffer.Length);

                    byte[] receiveData = new byte[bytesRead];
                    Array.Copy(buffer, receiveData, bytesRead);

                    cmds.Enqueue(receiveData);

                    isNewMessageReceived_ = true;
                    
                }
            }

            catch (ThreadAbortException)
            {
                break;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
                continue;
            }
        }
    }

    public bool TryDequeue(out byte[] result)
    {
        return cmds.TryDequeue(out result);
    }

    public void Write(string message)
    {
        try
        {
            serialPort_.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

}