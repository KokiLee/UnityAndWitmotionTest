using UnityEngine;
using UnityEngine.UI;

public class DataDisplay : MonoBehaviour
{
    public DataParser dataParser;

    public Text TextFrame;

    private float updateinterval = 0.1f;
    private float timeSinceLastUpdate = 0.0f;

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateinterval)
        {
            TextFrame.text = "Roll: " + string.Format("{0:F2}", (float)dataParser.roll) +
                            ", Pitch: " + string.Format("{0:F2}", (float)dataParser.pitch) +
                            ", Yaw: " + string.Format("{0:F2}", (float)dataParser.yaw);

            timeSinceLastUpdate = 0.0f;
        }
    }
}