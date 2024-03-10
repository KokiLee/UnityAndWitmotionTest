using UnityEngine;

public class Rotate : MonoBehaviour
{
    public DataParser dataParser;

    void Update()
    {
        // transformを取得
        Transform myTransform = this.transform;

        // ワールド座標を基準に、回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = (float)dataParser.pitch;

        worldAngle.z = (float)dataParser.roll;

        worldAngle.y = (float)dataParser.yaw;

        myTransform.eulerAngles = worldAngle;
    }
}