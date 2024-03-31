using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToggle : MonoBehaviour
{

    public Camera mainCamera;
    public Camera subCamera;
    public Toggle toggleButton;

    // Start is called before the first frame update
    void Start()
    {
        toggleButton.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            mainCamera.rect = new Rect(0.5f, 0, 0.5f, 1); // X,Y,Wide,Height
            subCamera.enabled = true;
        }
        else
        {
            mainCamera.rect = new Rect(0, 0, 1, 1); // X,Y,Wide,Height
            subCamera.enabled = false;
        }
    }

}
