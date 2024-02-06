using UnityEngine;

/* ###########################################################################################
 * カメラ追尾 
 * 
 * 
 * 
 * 
  #############################################################################################*/



public class Camera_Tracking : MonoBehaviour
{

    public GameObject playerObject;         //追尾 オブジェクト
    public Vector2 rotationSpeed;           //回転速度
    private Vector3 lastMousePosition;      //最後のマウス座標
    private Vector3 lastTargetPosition;     //最後の追尾オブジェクトの座標


    private float zoom;

    void Start()
    {
        zoom = 0.0f;
        lastMousePosition = Input.mousePosition;
        lastTargetPosition = playerObject.transform.position;
    }

    void Update()
    {

        Rotate();
        Zoom();
    }


    void Rotate()
    {
        transform.position = playerObject.transform.position; // カメラの位置を追尾オブジェクトに合わせる

        if (Input.GetMouseButton(1))
        {
            Vector3 nowMouseValue = Input.mousePosition - lastMousePosition;

            var newAngle = Vector3.zero;
            newAngle.x = rotationSpeed.x * nowMouseValue.x;
            newAngle.y = rotationSpeed.y * nowMouseValue.y;

            transform.RotateAround(playerObject.transform.position, Vector3.up, newAngle.x);
            transform.RotateAround(playerObject.transform.position, transform.right, -newAngle.y);
        }

        lastMousePosition = Input.mousePosition;
    }



    //拡大縮小
    void Zoom()
    {
        zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 offset = new Vector3(0, 0, 0);
        Vector3 pos = playerObject.transform.position - transform.position;

        if (zoom > 0)
        {
            offset = pos.normalized * 1;
        }
        else if (zoom < 0)
        {
            offset = -pos.normalized * 1;

        }
        transform.position = transform.position + offset;
    }


}
