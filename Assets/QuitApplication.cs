using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {

        Debug.Log("Quit method called"); // ���O�o��

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif


        Application.Quit();
    }
}