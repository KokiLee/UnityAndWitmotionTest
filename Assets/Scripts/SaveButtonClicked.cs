using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveButtonClicked : MonoBehaviour
{
    public TransFormPosition formPosition;
    public FreeCamCtrl freeCamCtrl;
    public Rotate rotate;
    public SerialHandler serial;

    public void OnUpdateButtonClicked()
    {
        float newPositionY = formPosition.positionY;
        float newRotationY = formPosition.rotationY;

        float newPlanesFar = freeCamCtrl.planesFarSlider.value;
        float newFieldOfView = freeCamCtrl.fieldOfViewSlider.value;
        bool newFxaaEnable = freeCamCtrl.fxaaEnable;

        float newLimitPitch = rotate.limitedPitchSlider.value;
        float newLimitRoll = rotate.limitedRollSlider.value;
        float newLimitYaw = rotate.limitedYawSlider.value;

        string newPortName = serial.serialPortName.text;

        formPosition.UpdateSettings(newPositionY, newRotationY);
        freeCamCtrl.UpdateSettings(newPlanesFar, newFieldOfView, newFxaaEnable);
        rotate.UpdateSettings(newLimitPitch, newLimitRoll, newLimitYaw);
        serial.UpdateSettings(newPortName);
    }
    public void OnCameraUpdateButtonClicked()
    {
        transform.position = freeCamCtrl.transform.position;
        transform.eulerAngles = freeCamCtrl.transform.eulerAngles;

        freeCamCtrl.UpdateCamera(transform);

    }
}
