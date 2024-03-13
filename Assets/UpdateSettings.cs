using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpdateSettings : MonoBehaviour
{

    public FreeCamCtrl freeCamCtrl;
    public SerialHandler serialHandler;
    public Rotate rotate;
    public TransFormPosition transFormPosition;

    public void UpdateJson()
    {
        if (freeCamCtrl != null) freeCamCtrl.LoadCameraSettings();
        if (serialHandler != null) serialHandler.LoadSerialSettings();
        if (transFormPosition != null) transFormPosition .LoadPositionValues();
        if (rotate != null) rotate .LoadLimitValues();
    }

}
