using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    public float limitedPitch = 30f;
    public float limitedRoll = 10f;
    public float limitedYaw = 360f;

    public Slider limitedPitchSlider;
    public Slider limitedRollSlider;
    public Slider limitedYawSlider;

    public TextMeshProUGUI limitedPitchValue;
    public TextMeshProUGUI limitedRollValue;
    public TextMeshProUGUI limitedYawValue;

    public FreeCamCtrl freeCamCtrl;
    public string filePath;

    private void Start()
    {
        LoadLimitValues();

        limitedPitchSlider.onValueChanged.AddListener(delegate { SettingPitch(); });
        limitedPitchSlider.value = limitedPitch;

        limitedRollSlider.onValueChanged.AddListener(delegate { SettingRoll(); });
        limitedRollSlider.value = limitedRoll;

        limitedYawSlider.onValueChanged.AddListener(delegate { SettingYaw(); });
        limitedYawSlider.value = limitedYaw;

        limitedPitchSlider.onValueChanged.AddListener(delegate { DisableCameraControl(); });
        limitedRollSlider.onValueChanged.AddListener(delegate { DisableCameraControl(); });
        limitedYawSlider.onValueChanged.AddListener(delegate { DisableCameraControl(); });

    }

    public void DisableCameraControl()
    {
        freeCamCtrl.isCameraContorolEnabled = false;
    }

    public void OnSliderDragEnd()
    {
        freeCamCtrl.isCameraContorolEnabled = true;
    }

    public void SettingPitch()
    {
        limitedPitch = limitedPitchSlider.value;

        limitedPitchValue.text = limitedPitchSlider.value.ToString("F2");
    }

    public void SettingRoll()
    {
        limitedRoll = limitedRollSlider.value;

        limitedRollValue.text = limitedRollSlider.value.ToString("F2");
    }

    public void SettingYaw()
    {
        limitedYaw = limitedYawSlider.value;

        limitedYawValue.text = limitedYawSlider.value.ToString("F2");
    }

    public void UpdateSettings(float newLimitPitch, float newLimitRoll, float newLimitYaw)
    {
        LimitAngleSettings settings;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<LimitAngleSettings>(dataAsJson);
        }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }

        settings.limitedPitch= newLimitPitch;
        settings.limitedRoll = newLimitRoll;
        settings.limitedYaw = newLimitYaw;

        string updatedJson = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Updated settings: LimitPitch and LimitRoll and LimitYaw.");
    }

    public void LoadLimitValues()
    {
        filePath = Path.Combine(Application.dataPath, "../Settings/limit_angle_settings.json");
        if (File.Exists(filePath))
        {
            Debug.Log("File found: " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            LimitAngleSettings settings = JsonUtility.FromJson<LimitAngleSettings>(dataAsJson);
            limitedPitch = settings.limitedPitch;
            limitedRoll = settings.limitedRoll;
            limitedYaw = settings.limitedYaw;

            limitedPitchValue.text = limitedPitch.ToString("F2");
            limitedRollValue.text = limitedRoll.ToString("F2");
            limitedYawValue.text = limitedYaw.ToString("F2");

            limitedPitchSlider.value = limitedPitch;
            limitedRollSlider.value = limitedRoll;
            limitedYawSlider.value = limitedYaw;

            freeCamCtrl.isCameraContorolEnabled = true;
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