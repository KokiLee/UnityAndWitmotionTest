using System;
using System.Collections.Concurrent;

public interface ISerialHandler
{
    bool isRunning_ { get; set; }
    ConcurrentQueue<byte[]> cmds { get; }

    void EnqueueData(byte[] data);

}