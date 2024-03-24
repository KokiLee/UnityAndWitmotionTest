using UnityEngine;
using System;


[Serializable]
public class CameraSettings
{
    public Vector3 position;
    public Vector3 rotation;
    public float fieldOfView = 72.0f;
    public float clippingPlanesFar = 6000.0f;
    public float clippingPlanesNear = 0.3f;
    public bool fxaaEnable = false;
}
