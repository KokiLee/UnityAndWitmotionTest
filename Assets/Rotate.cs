using UnityEngine;

public class Rotate : MonoBehaviour
{
    public SerialHandler serialHandler;

    void Start()
    {
        // 
    }

    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���[���h���W����ɁA��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = (float)serialHandler.pitch;
        worldAngle.z = (float)serialHandler.roll;
        worldAngle.y = (float)serialHandler.yaw;
        myTransform.eulerAngles = worldAngle;
    }
}