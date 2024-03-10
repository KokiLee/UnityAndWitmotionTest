using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Translate(0, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Translate(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Translate(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Translate(-1, 0, 0);
        }
    }
}
