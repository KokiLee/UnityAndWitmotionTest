using System;
using System.Collections.Concurrent;

public interface ISerialHandler
{
    bool isRunning_ { get; set; }
    ConcurrentQueue<byte[]> cmds { get; }
    Action OnPortOpened { get; set; }

    void EnqueueData(byte[] data);
    // �K�v�ɉ����đ��̃��\�b�h��v���p�e�B��ǉ�
}