using UnityEngine;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���[���h���W����ɁA��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = (float)dataParser.pitch;

        worldAngle.z = (float)dataParser.roll;

        worldAngle.y = (float)dataParser.yaw;

        myTransform.eulerAngles = worldAngle;
    }
}