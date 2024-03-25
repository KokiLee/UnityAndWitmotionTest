using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;



public class FreeCamCtrl : MonoBehaviour
{
    private Camera cam;
    private Vector3 lastMousePos;

    public float rotateSpeed = 5.0f;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 50.0f;
    public bool fxaaEnable = false;

    public Text CameraPositionText;

    public Slider planesFarSlider;
    public Slider fieldOfViewSlider;
    public TextMeshProUGUI planesFarValue;
    public TextMeshProUGUI filedOfViewValue;

    public Toggle antiAliasingEnable;

    public string filePath;

    public bool isCameraContorolEnabled = true;
    void Start()
    {
        cam = GetComponent<Camera>();
        LoadCameraSettings();
        planesFarSlider.onValueChanged.AddListener(delegate { SettingFar(); });
        planesFarSlider.value = cam.farClipPlane;

        fieldOfViewSlider.onValueChanged.AddListener(delegate { SettingFOV(); });
        fieldOfViewSlider.value = cam.fieldOfView;

        planesFarSlider.onValueChanged.AddListener(delegate { DisableCameraControl(); });
        fieldOfViewSlider.onValueChanged.AddListener(delegate { DisableCameraControl(); });

        antiAliasingEnable.onValueChanged.AddListener(delegate { SettingFXAA(); });

    }

    public void DisableCameraControl()
    {
        isCameraContorolEnabled = false;
    }

    public void OnSliderDragEnd()
    {
        isCameraContorolEnabled = true;
    }

    public void SettingFXAA()
    {
        var cameraData = cam.GetUniversalAdditionalCameraData();
        if (antiAliasingEnable.isOn)
        {
            fxaaEnable = true;
            cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
        }
        else
        {
            fxaaEnable = false;
            cameraData.antialiasing = AntialiasingMode.None;
        }
    }

    public void SettingFar()
    {
        cam.farClipPlane = planesFarSlider.value;

        planesFarValue.text = cam.farClipPlane.ToString("F0");
    }

    public void SettingFOV()
    {
        cam.fieldOfView = fieldOfViewSlider.value;

        filedOfViewValue.text = cam.fieldOfView.ToString("F0");
    }

    public void UpdateSettings(float newPlanesFar, float newFieldOfView, bool newFxaaEnable)
    {
        CameraSettings settings;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<CameraSettings>(dataAsJson);
        }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }

        settings.clippingPlanesFar = newPlanesFar;
        settings.fieldOfView = newFieldOfView;
        settings.fxaaEnable = newFxaaEnable;

        string updatedJson = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Updated settings: PlanesFar and FieldOfView.");
    }

    public void UpdateCamera(Transform newTransform)
    {
        CameraSettings settings;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<CameraSettings>(dataAsJson);
        }
        else
        {
            Debug.LogError("Settings file not found.");
            return;
        }

        settings.position = newTransform.position;
        settings.rotation = newTransform.eulerAngles;

        string updatedJson = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Updated settings: Camera Position and Rotation.");
    }

    public void LoadCameraSettings()
    {
        filePath = Path.Combine(Application.dataPath, "../Settings/camera_settings.json");
        if (File.Exists(filePath))
        {
            Debug.Log("File found: " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            CameraSettings settings = JsonUtility.FromJson<CameraSettings>(dataAsJson);
            transform.position = settings.position;
            transform.eulerAngles = settings.rotation;

            cam.fieldOfView = settings.fieldOfView;
            cam.farClipPlane = settings.clippingPlanesFar;
            cam.nearClipPlane = settings.clippingPlanesNear;

            fxaaEnable = settings.fxaaEnable;

            var cameraData = cam.GetUniversalAdditionalCameraData();

            if (fxaaEnable)
            {
                cameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
            }
            else
            {
                cameraData.antialiasing = AntialiasingMode.None;
            }

            planesFarValue.text = cam.farClipPlane.ToString("F0");
            filedOfViewValue.text = cam.fieldOfView.ToString("F0");
            planesFarSlider.value = cam.farClipPlane;
            fieldOfViewSlider.value = cam.fieldOfView;

            antiAliasingEnable.isOn = fxaaEnable;

            isCameraContorolEnabled = true;

        }
        else
        {
            Debug.LogError("Cannot find camera settings file.");
        }

    }

    void Update()
    {
        if (!isCameraContorolEnabled)
        {
            return;
        }

        if (cam == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            lastMousePos = Input.mousePosition;
        }

        // Rotation around the X axis(left click)
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.RotateAround(transform.position, Vector3.up, delta.x * rotateSpeed * Time.deltaTime);
        }

        // Rotation around the Y axis(right click)
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            transform.RotateAround(transform.position, transform.right, -delta.y * rotateSpeed * Time.deltaTime);
        }

        // Translation of camera(panning, center button)
        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            transform.Translate(move, Space.Self);
        }

        // Zoom (mouse wheel)
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        transform.Translate(0, 0, scroll, Space.Self);

        lastMousePos = Input.mousePosition;

        if (CameraPositionText.text != null)
        {
            CameraPositionText.text = $"CameraPosition: {transform.position}, CameraRotation: {transform.eulerAngles}";
        }
    }
}
