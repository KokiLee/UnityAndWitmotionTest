using System.Collections;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public SerialHandler serialHandler;

    public double roll = 0.0, pitch = 0.0, yaw = 0.0;

    private double preRoll = 0;
    private double prePitch = 0;
    private double preYaw = 0;

    // Start is called before the first frame update
    void Start()
    {
        serialHandler.OnPortOpened += StartProcessingData;
    }

    private void StartProcessingData()
    {
        StartCoroutine(ProcessDataCoroutine());
    }

    void OnDestroy()
    {
        serialHandler.OnPortOpened -= StartProcessingData;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static double AdjustData(double previous_data, double current_data)
    {

        double angleDiff = current_data - previous_data;

        angleDiff = ((angleDiff + 180) % 360) - 180;

        current_data = previous_data + angleDiff;

        current_data = ((current_data + 180) % 360) - 180;

        return current_data;
    }

    IEnumerator ProcessDataCoroutine()
    {

        while (serialHandler.isRunning_)
        {
            byte[] data = null;
            lock (serialHandler.cmds)
            {
                if (serialHandler.cmds.Count > 0)
                {
                    data = (byte[])serialHandler.cmds.Dequeue();
                    //Debug.Log(System.BitConverter.ToString(data) + "deta qua: " + serialHandler.cmds.Count);
                }
            }

            if (data != null)
            {

                for (int i = 0; i < data.Length - 1; i++)
                {
                    if (data[i] == 0x55 && data[i + 1] == 0x53)
                    {
                        roll = ((data[i + 3] << 8) | data[i + 2]) / 32768.0 * 180;
                        pitch = ((data[i + 5] << 8) | data[i + 4]) / 32768.0 * 180;
                        yaw = ((data[i + 7] << 8) | data[i + 6]) / 32768.0 * 180;

                        roll = AdjustData(preRoll, roll);
                        preRoll = roll;

                        pitch = AdjustData(prePitch, pitch);
                        prePitch = pitch;

                        yaw = AdjustData(preYaw, yaw);
                        preYaw = yaw;

                        Debug.Log("Roll: " + roll + ", Pitch: " + pitch + ", Yaw: " + yaw);

                    }
                }
            }

            yield return null;

        }
    }

}
