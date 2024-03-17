using UnityEngine;

public class UpdateSettings : MonoBehaviour
{

    public FreeCamCtrl freeCamCtrl;
    public ISerialHandler serialHandler;
    public Rotate rotate;
    public TransFormPosition transFormPosition;
    void Start()
    {
        if (serialHandler == null)
        {
            serialHandler = FindObjectOfType<SerialHandler>();
        }
    }
    public void UpdateJson()
    {
        if (freeCamCtrl != null) freeCamCtrl.LoadCameraSettings();
        if (serialHandler != null) serialHandler.LoadSerialSettings();
        if (transFormPosition != null) transFormPosition.LoadPositionValues();
        if (rotate != null) rotate.LoadLimitValues();
    }

}
