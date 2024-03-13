using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{

    public InputField inputField;
    public Text portName;

    public SerialHandler serialHandler;

    // Start is called before the first frame update
    void Start()
    {
        inputField = inputField.GetComponent<InputField>();
        portName = portName.GetComponent<Text>();
    }

    public void Inputtext()
    {
        portName.text = inputField.text;

        if (!string.IsNullOrEmpty(portName.text))
        {
            serialHandler.portName = string.Format(inputField.text);

            serialHandler.OpenPortWithNewName(portName.text);
        }
        else
        {
            Debug.LogWarning("Not input portName.");
        }
    }
}
