using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public Transform target;
    public Transform ShipObject;
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);

        Vector3 shipAngle = ShipObject.eulerAngles;

        //shipAngle.y = 90;

        //ShipObject.eulerAngles = shipAngle;
    }
}
