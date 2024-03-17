using System;
using System.Collections.Concurrent;

public interface ISerialHandler
{
    bool isRunning_ { get; set; }
    ConcurrentQueue<byte[]> cmds { get; }
    Action OnPortOpened { get; set; }

    void EnqueueData(byte[] data);
    // 必要に応じて他のメソッドやプロパティを追加
}