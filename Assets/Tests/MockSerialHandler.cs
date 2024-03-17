using System.Collections.Concurrent;
using UnityEngine;

public class MockSerialHandler : ISerialHandler
{
    public bool IsRunning_ { get; set; } = true;
    public ConcurrentQueue<byte[]> cmds { get; } = new ConcurrentQueue<byte[]>();

    // ISerialHandlerインターフェイスの他のメンバーを実装する必要があります。
    public void OpenPortWithNewName(string portName) { /* モックの振る舞いを実装 */ }
    public void Close() { /* モックの振る舞いを実装 */ }
    public event SerialHandler.PortOpenedHandler OnPortOpened;

    // テストデータをキューに追加するためのメソッド
    public void EnqueueData(byte[] data)
    {
        cmds.Enqueue(data);
    }

}