using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransFormPosition : MonoBehaviour
{

    public float positionX = 10.4f;
    public float positionY = 5.5f;
    public float positionZ = 16.8f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public Slider positionYslider;
    public Slider rotationYslider;
    public TextMeshProUGUI yAxisValue;
    public TextMeshProUGUI yAxisRotationValue;

    public FreeCamCtrl freeCamCtrl;
    public string filePath;

    private void Awake()
    {
        if (freeCamCtrl == null)
        {
            freeCamCtrl = FindObjectOfType<FreeCamCtrl>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPositionValues();
        transform.position = new Vector3(positionX, positionY, positionZ);
        transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        positionYslider.onValueChanged.AddListener(delegate { SettingPostionY(); });
        positionYslider.value = positionY;
        rotationYslider.onValueChanged.AddListener(delegate { SettingRotationY(); });
        rotationYslider.value = rotationY;

        positionYslider.onValueChanged.AddListener(delegate { DisableCameraControl(); });
        rotationYslider.onValueChanged.AddListener(delegate { DisableCameraControl(); });
    }

    public void DisableCameraControl()
    {
        freeCamCtrl.isCameraContorolEnabled = false;
    }

    public void OnSliderDragEnd()
    {
        freeCamCtrl.isCameraContorolEnabled = true;
    }


    public void LoadPositionValues()
    {
        filePath = Path.Combine(Application.dataPath, "../Settings/position_settings.json");
        if (File.Exists(filePath))
        {
            Debug.Log("File found: " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            PositionSettings settings = JsonUtility.FromJson<PositionSettings>(dataAsJson);
            positionX = settings.positionX;
            positionY = settings.positionY;
            positionZ = settings.positionZ;

            rotationX = settings.rotationX;
            rotationY = settings.rotationY;
            rotationZ = settings.rotationZ;

            transform.position = new Vector3(positionX, positionY, positionZ);
            transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);

            yAxisValue.text = positionY.ToString("F2");
            yAxisRotationValue.text = rotationY.ToString("F2");
            positionYslider.value = positionY;
            rotationYslider.value = rotationY;

            freeCamCtrl.isCameraContorolEnabled = true;
        }
        else
        {
            Debug.LogError("Cannot find camera settings file.");
        }
    }

    public void SettingPostionY()
    {
        Vector3 newYposition = transform.position;
        positionY = positionYslider.value;

        yAxisValue.text = positionY.ToString("F2");

        newYposition.y = positionY;
        transform.position = newYposition;
    }
    
    public void SettingRotationY()
    {
        Vector3 newYrotation = transform.eulerAngles;
        rotationY = rotationYslider.value;

        yAxisRotationValue.text = rotationY.ToString("F2");

        newYrotation.y = rotationY;
        transform.eulerAngles = newYrotation;

    }

    public void UpdateSettings(float newPositionY, float newRotationY)
    {
        PositionSettings settings;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<PositionSettings>(dataAsJson);
                    }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }

        settings.positionY = newPositionY;
        settings.rotationY = newRotationY;

        string updatedJson = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Updated settings: PositionY and RotationY.");
    }

}
