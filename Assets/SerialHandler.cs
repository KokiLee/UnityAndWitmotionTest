using System;
using System.IO;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;


public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    public string portName = "COM3";
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    public bool isRunning_ = false;

    public Queue cmds = new Queue();


    private string message_;
    private volatile bool isNewMessageReceived_ = false;

    public delegate void PortOpenedHandler();
    public event PortOpenedHandler OnPortOpened;

    byte[] buffer = new byte[100];

    void Start()
    {
        LoadSerialSettings();
        OpenPortWithNewName(portName);
    }

    private void TryReconnect()
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                OpenPortWithNewName(portName);
                if (serialPort_.IsOpen)
                {
                    Debug.Log("Reconnected to serial port.");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to reconnect: {e.Message}");
                Thread.Sleep(1000);
            }
        }
        Debug.LogError("Failed to reconnect to serial port.");
    }

    private void LoadSerialSettings()
    {
        string path = Path.Combine(Application.dataPath, "../Settings/serial_settings.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SerialSettings settings = JsonUtility.FromJson<SerialSettings>(json);
            portName = settings.portName;
            baudRate = settings.baudRate;
            Debug.Log(portName + baudRate);
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
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        serialPort_.Open();
        serialPort_.ReadTimeout = 50;

        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();

        // Notification port is open
        OnPortOpened?.Invoke();
    }

    private void Close()
    {
        isNewMessageReceived_ = false;
        isRunning_ = false;

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

        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                if (serialPort_.BytesToRead > 0)
                {
                    Array.Clear(buffer, 0, buffer.Length);

                    int bytesRead = serialPort_.Read(buffer, 0, buffer.Length);

                    byte[] receiveData = new byte[bytesRead];
                    Array.Copy(buffer, receiveData, bytesRead);

                    lock (cmds)
                    {
                        cmds.Enqueue(receiveData);
                    }

                    isNewMessageReceived_ = true;
                }
            }

            catch (ThreadAbortException)
            {
                break;
            }
            //catch (IOException)
            //{
            //    Debug.LogWarning("Serial port disconnected.");
            //    Close();
            //    TryReconnect();
            //    break;
            //}
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
                continue;
            }
        }
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