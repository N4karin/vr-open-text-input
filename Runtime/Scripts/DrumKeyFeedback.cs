using UnityEngine;
using TMPro;

public class DrumKeyFeedback : MonoBehaviour
{
    private string label;
    private bool isPressed;
    private Color defaultColor;
    private Color pressedColor;
    private KeyAudioFeedback soundHandler;
    private DrumKeyController parentKeyController;
    private BoxCollider boxCol;
    private Vector3 originalBoxColSize;
    private Renderer _renderer;
    
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
        boxCol = gameObject.GetComponent<BoxCollider>();
        originalBoxColSize = boxCol.size;
        parentKeyController = transform.parent.gameObject.GetComponent<DrumKeyController>();
        _renderer = GetComponent<Renderer>();
        defaultColor = parentKeyController.getDefaultColor();
        pressedColor = parentKeyController.getPressedColor();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("KeyboardTrigger") && parentKeyController.getKeyReset())
        {
            // Block other keys from being pressed
            parentKeyController.setKeyReset(false);
            isPressed = true;
            
            // Transmit input event to script in parent object
            switch (label)
            {
                case "BackSpace": 
                    parentKeyController.BackspacePressed(); 
                    break;
                
                case "Shift":
                    if (parentKeyController.getShiftPressed())
                    {
                        return;
                    }
                    parentKeyController.setShiftPressed(true);
                    parentKeyController.setKeyReset(true);
                    break;
                
                case "Return":
                    parentKeyController.ReturnPressed();
                    break;

                default:
                    parentKeyController.KeyPressed(label);
                    break;
            }
            
            // Change color 
            _renderer.material.color = pressedColor;
            
            // Play sound
            soundHandler.PlayKeyClick();
            
            // Scale box collider up to prevent multiple inputs
            
            if (label != "Shift")
            {
                boxCol.size += new Vector3(0.2f, 10f, 0.2f);
            }
            else
            {
                boxCol.size += new Vector3(0, 10f, 0);
            }

            // Displace on click
            transform.localScale += new Vector3(0.01f, 0, 0.01f);
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("KeyboardTrigger") && isPressed)
        {
            if (label == "Shift")
            {
                parentKeyController.setShiftPressed(false);
            }
            
            // Scale box collider to match actual mesh again
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size = originalBoxColSize;

            // Allow other keys to be pressed again
            parentKeyController.setKeyReset(true);
            isPressed = false;
            _renderer.material.color = defaultColor;

            // Displace on release
            transform.localScale -= new Vector3(0.01f, 0, 0.01f);
        }
    }
}
