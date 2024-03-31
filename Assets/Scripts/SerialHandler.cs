using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SerialHandler : MonoBehaviour, ISerialHandler
{
    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;

    public TMP_InputField serialPortName;
    public string filePath;
    

    // Thread safe queue
    public ConcurrentQueue<byte[]> cmds { get; set; } = new();

    private string message_;

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

        serialPortName.onEndEdit.AddListener(delegate { SettingPort();});
    }

    public void SettingPort()
    {
        portName = serialPortName.text.ToUpper();

        serialPortName.text = portName;

        OpenPortWithNewName(portName);
    }

    public void UpdateSettings(string newPortName)
    {
        SerialSettings settings;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<SerialSettings>(dataAsJson);
        }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }

        settings.portName = newPortName;

        string updatedJson = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Updated settings: PortName.");
    }

    public void LoadSerialSettings()
    {
        filePath = Path.Combine(Application.dataPath, "../Settings/serial_settings.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SerialSettings settings = JsonUtility.FromJson<SerialSettings>(json);
            portName = settings.portName;
            baudRate = settings.baudRate;
            serialPortName.text = portName;
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
            Debug.Log(string.Join("\n", availablePorts));
            

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
        IsRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Interrupt();

            if (!thread_.Join(1000))
            {
                Debug.LogWarning("Thread did not finish in time");
            }
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
            catch (IOException e)
            {
                Debug.LogWarning($"IOException caught: {e.Message}");
                IsRunning_ = false;
                Close();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
                IsRunning_ = false;
                Close();
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

    // Use this method for testing only
    public void EnqueueData(byte[] data)
    {
        cmds.Enqueue(data);
    }
}