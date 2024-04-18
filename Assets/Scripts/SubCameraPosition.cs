using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCameraPosition : MonoBehaviour
{

    public Camera subCamera;
    public Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = 185.00f;

        Vector3 cameraPosition = targetTransform.position + (targetTransform.right * distanceToTarget);

        cameraPosition.y = 45;

        transform.position = cameraPosition;

        transform.LookAt(targetTransform);
    }
}
