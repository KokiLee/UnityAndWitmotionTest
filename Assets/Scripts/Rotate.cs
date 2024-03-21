using System.IO;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    public float limitedPitch = 30f;
    public float limitedRoll = 10f;
    public float limitedYaw = 360f;


    private void Start()
    {
        LoadLimitValues();
    }

    public void LoadLimitValues()
    {
        string filePath = Path.Combine(Application.dataPath, "../Settings/limit_angle_settings.json");
        if (File.Exists(filePath))
        {
            Debug.Log("File found: " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            LimitAngleSettings settings = JsonUtility.FromJson<LimitAngleSettings>(dataAsJson);
            limitedPitch = settings.limitedPitch;
            limitedRoll = settings.limitedRoll;
            limitedYaw = settings.limitedYaw;
        }
        else
        {
            Debug.LogError("Cannot find camera settings file.");
        }
    }

    void Update()
    {
        Transform myTransform = this.transform;

        float pitch = Mathf.Clamp((float)dataParser.pitch, -limitedPitch, limitedPitch);
        float roll = Mathf.Clamp((float)dataParser.roll, -limitedRoll, limitedRoll);
        float yaw = Mathf.Clamp((float)dataParser.yaw, -limitedYaw, limitedYaw);

        // Get rotation based on world coordinates.
        Vector3 worldAngle = myTransform.eulerAngles;

        if (limitedPitch != 0.0)
        {
            worldAngle.x = -pitch;
        }

        if (limitedRoll != 0.0)
        {
            worldAngle.z = roll;
        }

        if (limitedYaw != 0.0)
        {
            worldAngle.y = yaw;
        }

        myTransform.eulerAngles = worldAngle;
    }
}