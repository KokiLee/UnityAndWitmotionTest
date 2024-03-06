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
        // transformを取得
        Transform myTransform = this.transform;

        // ワールド座標を基準に、回転を取得
        Vector3 worldAngle = myTransform.eulerAngles;
        worldAngle.x = (float)serialHandler.pitch;
        worldAngle.z = (float)serialHandler.roll;
        worldAngle.y = (float)serialHandler.yaw;
        myTransform.eulerAngles = worldAngle;
    }
}