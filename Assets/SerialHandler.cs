using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using UnityEngine;


public class SerialHandler : MonoBehaviour
{
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;



    //ポート名
    public string portName = "COM3";// 自分の使用したいセンサを確認し，適切なCOM番号に変更してください
    public int baudRate = 9600;

    private SerialPort serialPort_;
    private Thread thread_;
    private bool isRunning_ = false;

    public Queue cmds = new Queue();


    private string message_;
    private bool isNewMessageReceived_ = false;

    private string msg = "";
    public double roll = 0.0, pitch = 0.0, yaw = 0.0;

    byte[] buffer = new byte[100];
    int cnt = 0;

    void Awake()
    {
        Open();
    }

    void Update()
    {
        // nothing
    }

    void OnDestroy()
    {
        Close();
    }

    public void Open()
    {
        serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        serialPort_.Open();
        serialPort_.ReadTimeout = 50;

        isRunning_ = true;

        thread_ = new Thread(Read);
        thread_.Start();
    }

    private void Close()
    {
        isNewMessageReceived_ = false;
        isRunning_ = false;

        if (thread_ != null && thread_.IsAlive)
        {
            thread_.Join();
        }

        if (serialPort_ != null && serialPort_.IsOpen)
        {
            serialPort_.Close();
            serialPort_.Dispose();
        }

    }

    private void Read()
    {
        int plus = 0;
        int minus = 0;
        double preRoll = 0;
        double roll_diff = 0;

        // COMPort確認
        // string[] ports = SerialPort.GetPortNames();
        // foreach(string port in ports){
        //     // Debug.Log(port);
        // }

        while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
        {
            try
            {
                serialPort_.Read(buffer, 0, buffer.Length);

                isNewMessageReceived_ = true;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0x55 && buffer[i + 1] == 0x53)
                {
                    int start = i + 2;
                    this.roll = (buffer[start + 1] * Math.Pow(2, 8) + buffer[start + 0]) / 32768.0 * 180;
                    this.pitch = (buffer[start + 3] * Math.Pow(2, 8) + buffer[start + 2]) / 32768.0 * 180;
                    this.yaw = (buffer[start + 5] * Math.Pow(2, 8) + buffer[start + 4]) / 32768.0 * 180;


                    // センサ値を[-180,180]に変換する
                    if (this.roll > 180)
                    {
                        this.roll -= 360;
                    }
                    // センサ値を[0→90→0],[0→-90→0]に変換する
                    if (this.pitch > 180)
                    {
                        this.pitch -= 360;
                    }

                    Debug.Log(this.pitch);

                    // センサ値を[-180,180]に変換する
                    // if (yaw > 180) {
                    //     yaw -= 360;
                    // }

                    if (preRoll != 0)
                    {
                        roll_diff = this.roll - preRoll;
                        if (roll_diff > 180)
                        {
                            roll -= 360;

                        }
                        else if (roll_diff < -180)
                        {
                                this.roll += 360;
                        }
                     }

                    preRoll = this.roll;
                    

                    // Check
                    // Debug.Log($"roll:{roll}");
                    // Debug.Log($"pitch:{pitch}");
                    // Debug.Log($"yaw:{yaw}");
                }
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