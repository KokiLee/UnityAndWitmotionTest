using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveButtonClicked : MonoBehaviour
{
    public TransFormPosition formPosition;

    public void OnUpdateButtonClicked()
    {
        float newPositionY = formPosition.positionY;
        float newRotationY = formPosition.rotationY;

        formPosition.UpdateSettings(newPositionY, newRotationY);
    }
}
