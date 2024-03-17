using System.Collections.Concurrent;

public class MockSerialHandler : ISerialHandler
{
    public bool IsRunning_ { get; set; } = true;
    public ConcurrentQueue<byte[]> cmds { get; } = new ConcurrentQueue<byte[]>();
    public string portName { get; set; }
    public int baudRate { get; set; }
    // ISerialHandlerインターフェイスの他のメンバーを実装する必要があります。
    public void OpenPortWithNewName(string portName) { /* モックの振る舞いを実装 */ }
    public void Close() { /* モックの振る舞いを実装 */ }
    public event SerialHandler.SerialStatusChangedHandler OnSerialStatusChanged;

    // テストデータをキューに追加するためのメソッド
    public void EnqueueData(byte[] data)
    {
        cmds.Enqueue(data);
    }

    public void SerialStatusChanged(bool isRunning)
    {
        IsRunning_ = isRunning;
        OnSerialStatusChanged?.Invoke(isRunning);
    }

    public void LoadSerialSettings()
    {
        string testPortName = "COM3";
        int testBaudRate = 9600;

        this.portName = testPortName;
        this.baudRate = testBaudRate;
    }
}