using UnityEngine;
using TMPro;

public class KeyFeedback : MonoBehaviour
{
    private string label;
    private bool isPressed;
    private Color defaultColor;
    private Color pressedColor;
    private KeyAudioFeedback soundHandler;
    private KeyController parentKeyController;
    private BoxCollider boxCol;
    private Vector3 originalBoxColSize;
    private Renderer _renderer;
    
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
        boxCol = gameObject.GetComponent<BoxCollider>();
        originalBoxColSize = boxCol.size;
        parentKeyController = transform.parent.gameObject.GetComponent<KeyController>();
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
                
                case "→":
                    parentKeyController.RightArrowPressed();
                    break;
                
                case "←":
                    parentKeyController.LeftArrowPressed();
                    break;
                
                case "Tab":
                    parentKeyController.TabulatorPressed();
                    break;
                
                case "CapsLock":
                    parentKeyController.ToggleCaps();
                    break;
                
                default:
                    parentKeyController.KeyPressed(label);
                    break;
            }
            
            // Change color 
            _renderer.material.color = pressedColor;
            
            // Play sound
            soundHandler.PlayKeyClick();
            
            // Scale box collider up to prevent multiple inputs if not Shift
            if (label != "Shift")
            {
                boxCol.size += new Vector3(20, 100, 20);
            }
            else
            {
                boxCol.size += new Vector3(0, 100, 0);
            }

            // Displace on click
            transform.position += transform.rotation * new Vector3(0, -0.005f, 0);
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
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0);
        }
    }
}
