using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    public void Quit()
    {

        Debug.Log("Quit method called"); // ÉçÉOèoóÕ

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif


        Application.Quit();
    }
}