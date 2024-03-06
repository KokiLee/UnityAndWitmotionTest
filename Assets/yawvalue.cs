using UnityEngine;
using UnityEngine.UI;

public class yawvalue : MonoBehaviour
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
        TextFrame.text = string.Format("{00:0000}", (float)serialHandler.yaw);
        myTransform.eulerAngles = worldAngle;
    }
}
