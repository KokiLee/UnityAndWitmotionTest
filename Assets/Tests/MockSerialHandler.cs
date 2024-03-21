using System.Collections.Concurrent;

public class MockSerialHandler : ISerialHandler
{
    public bool IsRunning_ { get; set; } = true;
    public ConcurrentQueue<byte[]> cmds { get; } = new ConcurrentQueue<byte[]>();
    public string portName { get; set; }
    public int baudRate { get; set; }

    public void OpenPortWithNewName(string portName) { /* Implement mock behavior */ }
    public void Close() { /* Implement mock behavior */ }
    public event SerialHandler.SerialStatusChangedHandler OnSerialStatusChanged;

    // Method for adding test data to the queue.
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
        string testPortName = "COM1";
        int testBaudRate = 9600;

        this.portName = testPortName;
        this.baudRate = testBaudRate;
    }

    public void OpenPort()
    {
        OpenPortWithNewName(this.portName);
    }

}