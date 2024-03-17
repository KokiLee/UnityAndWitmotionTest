using System.Collections.Concurrent;

public class MockSerialHandler : ISerialHandler
{
    public bool IsRunning_ { get; set; } = true;
    public ConcurrentQueue<byte[]> cmds { get; } = new ConcurrentQueue<byte[]>();
    public string portName { get; set; }
    public int baudRate { get; set; }
    // ISerialHandler�C���^�[�t�F�C�X�̑��̃����o�[����������K�v������܂��B
    public void OpenPortWithNewName(string portName) { /* ���b�N�̐U�镑�������� */ }
    public void Close() { /* ���b�N�̐U�镑�������� */ }
    public event SerialHandler.SerialStatusChangedHandler OnSerialStatusChanged;

    // �e�X�g�f�[�^���L���[�ɒǉ����邽�߂̃��\�b�h
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