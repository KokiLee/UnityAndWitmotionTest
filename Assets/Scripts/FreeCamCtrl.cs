using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;



public class FreeCamCtrl : MonoBehaviour
{
    private Camera cam;
    private Vector3 lastMousePos;

    public float rotateSpeed = 5.0f;
    public float panSpeed = 0.5f;
    public float zoomSpeed = 50.0f;
    public bool fxaaEnable = false;

    public Text CameraPositionText;

    public bool isCameraContorolEnabled = true;
    void Start()
    {
        cam = GetComponent<Camera>();
        LoadCameraSettings();
    }

    public void LoadCameraSettings()
    {
        string filePath = Path.Combine(Application.dataPath, "../Settings/camera_settings.json");
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
