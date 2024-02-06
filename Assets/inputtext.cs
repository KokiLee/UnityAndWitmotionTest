using UnityEngine;
using UnityEngine.UI;

public class inputtext : MonoBehaviour
{

    public InputField inputField;
    public Text text;

    public SerialHandler serialHandler;

    // Start is called before the first frame update
    void Start()
    {
        inputField = inputField.GetComponent<InputField>();
        text = text.GetComponent<Text>();
    }

    public void Inputtext()
    {
        text.text = inputField.text;

        serialHandler.portName = string.Format(inputField.text);

        serialHandler.Open();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
