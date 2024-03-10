using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    private Camera cam;
    private Vector3 startPos;
    private Vector3 startAngle;
    public GameObject targetobject;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.transform.LookAt(targetobject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            return;
        }

        float sensitiveMove = 5.0f;
        float sensitiveRotate = 5.0f;
        float sensitiveZoom = 50.0f;

        if (Input.GetMouseButton(0))
        {
            // move camera
            float moveX = Input.GetAxis("Mouse X") * sensitiveMove;
            cam.transform.RotateAround(targetobject.transform.position, Vector3.up, moveX);

        }
        else if (Input.GetMouseButton(1))
        {
            // rotate camera
            float rotateY = -Input.GetAxis("Mouse Y") * sensitiveRotate;
            cam.transform.RotateAround(targetobject.transform.position, Vector3.right, rotateY);
        }

        // zoom camera
        float moveZ = Input.GetAxis("Mouse ScrollWheel") * sensitiveZoom;
        cam.transform.position += cam.transform.forward * moveZ;

        cam.transform.LookAt(targetobject.transform.position);
    }
}
