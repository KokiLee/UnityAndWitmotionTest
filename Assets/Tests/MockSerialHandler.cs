using System.Collections.Concurrent;
using UnityEngine;

public class MockSerialHandler : ISerialHandler
{
    public bool IsRunning_ { get; set; } = true;
    public ConcurrentQueue<byte[]> cmds { get; } = new ConcurrentQueue<byte[]>();

    // ISerialHandler�C���^�[�t�F�C�X�̑��̃����o�[����������K�v������܂��B
    public void OpenPortWithNewName(string portName) { /* ���b�N�̐U�镑�������� */ }
    public void Close() { /* ���b�N�̐U�镑�������� */ }
    public event SerialHandler.PortOpenedHandler OnPortOpened;

    // �e�X�g�f�[�^���L���[�ɒǉ����邽�߂̃��\�b�h
    public void EnqueueData(byte[] data)
    {
        cmds.Enqueue(data);
    }

}