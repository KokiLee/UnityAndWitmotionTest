using System.Collections;
using UnityEngine;


public class DataParser : MonoBehaviour
{
    public ISerialHandler serialHandler;

    public double roll = 0.0, pitch = 0.0, yaw = 0.0;

    private double preRoll = 0;
    private double prePitch = 0;
    private double preYaw = 0;

    public bool isCoroutineRunning = false;
    private bool startCoroutineFlag = false;
    private bool stopCoroutineFlag = false;
    private Coroutine processDataCoroutine;
    private void Awake()
    {
        serialHandler = GetComponent<SerialHandler>();
        if (serialHandler == null)
        {
            serialHandler = gameObject.AddComponent<SerialHandler>();
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        serialHandler.OnSerialStatusChanged += SerialStatusChanged;
        Debug.Log($"Start, serial: {serialHandler}, {serialHandler.IsRunning_}");
    }

    private void OnDisable()
    {
        serialHandler.OnSerialStatusChanged -= SerialStatusChanged;
    }

    private void Update()
    {
        if (startCoroutineFlag)
        {
            if (processDataCoroutine == null)
            {
                processDataCoroutine = StartCoroutine(ProcessDataCoroutine());
            }
            startCoroutineFlag = false;
        }

        if (stopCoroutineFlag)
        {
            if (processDataCoroutine != null)
            {
                StopCoroutine(processDataCoroutine);
                processDataCoroutine = null;
            }
            stopCoroutineFlag = false;
        }
    }

    private void SerialStatusChanged(bool isRunning)
    {
        if (isRunning)
        {
            startCoroutineFlag = true;
            stopCoroutineFlag = false;
        }
        else
        {
            stopCoroutineFlag = true;
            startCoroutineFlag = false;
        }
    }


    private void OnDestroy()
    {
        serialHandler.OnSerialStatusChanged -= SerialStatusChanged;
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

    public IEnumerator ProcessDataCoroutine()
    {

        Debug.Log("ProcessDataCoroutine has started.");

        while (serialHandler.IsRunning_)
        {

            try
            {

                while (serialHandler.cmds.TryDequeue(out byte[] data))
                {
                    for (int i = 0; i < data.Length - 1; i++)
                    {

                        if (data[i] == 0x55 && data[i + 1] == 0x53)
                        {

                            if (i + 7 >= data.Length) continue;

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

            catch (System.Exception e)
            {

                Debug.LogWarning(e.Message);

            }


            yield return null;

        }

        Debug.Log("ProcessDataCoroutine ended.");

    }

}
