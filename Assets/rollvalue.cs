using UnityEngine;
using UnityEngine.UI;

public class rollvalue : MonoBehaviour
{
    public SerialHandler serialHandler;

    public Text TextFrame;

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
        TextFrame.text = string.Format("{00:0000}", (float)serialHandler.roll);
        myTransform.eulerAngles = worldAngle;
    }
}
