using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

public class SerialHandler : MonoBehaviour, ISerialHandler
{
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;


    // Thread safe queue
    public ConcurrentQueue<byte[]> cmds { get; set; } = new();

    private string message_;
    //private volatile bool isNewMessageReceived_ = false;


    //public delegate void PortOpenedHandler();
    //public event PortOpenedHandler OnSerialStatusChanged;

    private bool isRunning;
    public delegate void SerialStatusChangedHandler(bool isRunning);
    public event SerialStatusChangedHandler OnSerialStatusChanged;
    
    
    public bool IsRunning_
    {
        get { return isRunning; }
        set
        {
            if (isRunning != value)
            {
                isRunning = value;
                OnSerialStatusChanged?.Invoke(isRunning);
            }
        }
    }


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
            string[] availablePorts = SerialPort.GetPortNames();

            if (!availablePorts.Contains(portName))
            {
                Debug.LogError($"Port {portName} is not available.");
                return;
            }

            serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
            {
                ReadTimeout = 50
            };

            serialPort_.Open();
            Debug.Log("Port Open");

            IsRunning_ = true;

            thread_ = new Thread(Read);
            thread_.Start();
        }
        catch (UnauthorizedAccessException e)
        {
            Debug.LogError($"UnauthorizedAccessException: {e.Message}");
            IsRunning_ = false;
        }
        catch (IOException e)
        {
            Debug.LogError($"IOException: {e.Message}");
            IsRunning_ = false;
        }
        catch (ArgumentException e)
        {
            Debug.LogError($"ArgumentException: {e.Message}");
            IsRunning_ = false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open serial port {portName}: {e.Message}");
            IsRunning_ = false;
        }
    }

    public void Close()
    {
        //isNewMessageReceived_ = false;
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
                    Debug.Log("Port Close");
                }
            }
            finally
            {
                serialPort_.Dispose();
                Debug.Log("Port Dispose");
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

                    //isNewMessageReceived_ = true;

                }
                else
                {
                    Thread.Sleep(50);
                }
            }

            catch (ThreadAbortException)
            {
                Close();
                break;
            }
            //catch (IOException e)
            //{
            //    Debug.LogWarning($"IOException caught: {e.Message}");
            //    IsRunning_ = false;
            //    Close();
            //}
            //catch (System.Exception e)
            //{
            //    Debug.LogWarning(e.Message);
            //    IsRunning_ = false;
            //    Close();
            //}
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

    // Use this method for testing only
    public void EnqueueData(byte[] data)
    {
        cmds.Enqueue(data);
    }
}