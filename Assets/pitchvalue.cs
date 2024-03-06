using UnityEngine;
using UnityEngine.UI;

public class pitchvalue : MonoBehaviour
{
    public SerialHandler serialHandler;

    public Text TextFrame;

    void Start()
    {
        // 
    }

    void Update()
    {
        // transform‚ðŽæ“¾
        Transform myTransform = this.transform;

        Vector3 worldAngle = myTransform.eulerAngles;
        TextFrame.text = string.Format("{00:0000}", (float)serialHandler.pitch);
        myTransform.eulerAngles = worldAngle;
    }
}