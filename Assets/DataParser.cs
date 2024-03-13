using System.Collections;
using System;
using UnityEngine;

public class DataParser : MonoBehaviour
{
    public SerialHandler serialHandler;

    public double roll = 0.0, pitch = 0.0, yaw = 0.0;

    private double preRoll = 0;
    private double prePitch = 0;
    private double preYaw = 0;

    // Start is called before the first frame update
    private void Start()
    {
        serialHandler.OnPortOpened += StartProcessingData;
    }

    private void StartProcessingData()
    {
        StartCoroutine(ProcessDataCoroutine());
    }

    private void OnDestroy()
    {
        serialHandler.OnPortOpened -= StartProcessingData;
    }

    private static double AdjustData(double previous_data, double current_data)
    {
        double angleDiff = current_data - previous_data;

        // Normalize angle difference to range from -180 degrees to 180 degrees.
        angleDiff = (angleDiff + 180) % 360 - 180;
        if (angleDiff < -180) angleDiff += 360; // Currently handle negative differences over -360 degrees.

        // Update current angle using normalized difference
        current_data = previous_data + angleDiff;

        // The current angle is also normalized to the range -180 degress to 180 degrees.
        current_data = (current_data + 180) % 360 - 180;
        if (current_data < -180) current_data += 360;

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
                }
            }

            if (data != null)
            {

                for (int i = 0; i < data.Length - 1; i++)
                {
                    if (data.Length >= 6)
                    {
                        if (data[i] == 0x55 && data[i + 1] == 0x53)
                        {

                            byte[] roll_L_H = { data[i + 2], data[i + 3] };
                            short combined_roll = System.BitConverter.ToInt16(roll_L_H, 0);

                            byte[] pitch_L_H = { data[i + 4], data[i + 5] };
                            short combined_pitch = System.BitConverter.ToInt16(pitch_L_H, 0);

                            byte[] yaw_L_H = { data[i + 6], data[i + 7] };
                            short combined_yaw = System.BitConverter.ToInt16(yaw_L_H, 0);

                            roll = combined_roll / 32768.0 * 180.0;
                            pitch = combined_pitch / 32768.0 * 180.0;
                            yaw = combined_yaw / 32768.0 * 180.0;

                            roll = AdjustData(preRoll, roll);
                            preRoll = roll;

                            pitch = AdjustData(prePitch, pitch);
                            prePitch = pitch;

                            yaw = AdjustData(preYaw, yaw);
                            preYaw = yaw;
                        }
                    }
                }
            }

            yield return null;

        }
    }

}
