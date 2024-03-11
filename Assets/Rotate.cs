using UnityEngine;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;


        float pitch = Mathf.Clamp((float)dataParser.pitch, -30f, 30f);
        float roll = Mathf.Clamp((float)dataParser.roll, -10f, 10f);
        float yaw = Mathf.Clamp((float)dataParser.yaw, -360f, 360f);

        // ���[���h���W����ɁA��]���擾
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = pitch;

        worldAngle.z = roll;

        worldAngle.y = yaw;

        myTransform.eulerAngles = worldAngle;
    }
}