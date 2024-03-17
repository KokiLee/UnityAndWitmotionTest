using System;
using System.Collections.Concurrent;

public interface ISerialHandler
{
    bool IsRunning_ { get; set; }
    void OpenPortWithNewName(string portName);
    void Close();

    event SerialHandler.PortOpenedHandler OnPortOpened;
    ConcurrentQueue<byte[]> cmds { get; }

    void EnqueueData(byte[] data);

}