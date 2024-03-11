using UnityEngine;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;


        float pitch = Mathf.Clamp((float)dataParser.pitch, -30f, 30f);
        float roll = Mathf.Clamp((float)dataParser.roll, -10f, 10f);
        float yaw = Mathf.Clamp((float)dataParser.yaw, -360f, 360f);

        // ワールド座標を基準に、回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = pitch;

        worldAngle.z = roll;

        worldAngle.y = yaw;

        myTransform.eulerAngles = worldAngle;
    }
}