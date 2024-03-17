using System.Collections.Concurrent;


public interface ISerialHandler
{
    bool IsRunning_ { get; set; }
    void OpenPortWithNewName(string portName);
    void Close();

    event SerialHandler.SerialStatusChangedHandler OnSerialStatusChanged;
    ConcurrentQueue<byte[]> cmds { get; }


    void EnqueueData(byte[] data);
    void LoadSerialSettings();
}