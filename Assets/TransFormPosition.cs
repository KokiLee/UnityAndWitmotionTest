using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TransFormPosition : MonoBehaviour
{

    public float positionX = 10.4f;
    public float positionY = 5.5f;
    public float positionZ = 16.8f;

    // Start is called before the first frame update
    void Start()
    {
        LoadPositionValues();
        transform.position = new Vector3(positionX, positionY, positionZ);
    }

    public void LoadPositionValues()
    {
        string filePath = Path.Combine(Application.dataPath, "../Settings/position_settings.json");
        if (File.Exists(filePath))
        {
            Debug.Log("File found: " + filePath);
            string dataAsJson = File.ReadAllText(filePath);
            PositionSettings settings = JsonUtility.FromJson<PositionSettings>(dataAsJson);
            positionX = settings.positionX;
            positionY = settings.positionY;
            positionZ = settings.positionZ;

            transform.position = new Vector3(positionX, positionY, positionZ);
        }
        else
        {
            Debug.LogError("Cannot find camera settings file.");
        }
    }
}
