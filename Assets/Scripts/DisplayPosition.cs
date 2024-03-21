using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayPosition : MonoBehaviour
{

    public TextMeshProUGUI ObjectPosition;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 rot = transform.eulerAngles;

        ObjectPosition.text = $"Position: {pos.x:F2}, {pos.y:F2}, {pos.z:F2} Rotation: {rot.x:F2}, {rot.y:F2}, {rot.z:F2}";
    }
}

