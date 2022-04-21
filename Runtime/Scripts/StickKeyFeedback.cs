using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StickKeyFeedback : MonoBehaviour
{
    private string label;
    private bool isPressed;
    private Color defaultColor;
    private Color pressedColor;
    private KeyAudioFeedback soundHandler;
    private AnalogKeyboardController parentKeyController;
    private Renderer _renderer;
    private Vector3 originalScale;
    
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
        parentKeyController = transform.parent.parent.gameObject.GetComponent<AnalogKeyboardController>();
        _renderer = GetComponent<Renderer>();
        defaultColor = parentKeyController.getDefaultColor();
        pressedColor = parentKeyController.getPressedColor();
        originalScale = transform.localScale;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("KeyboardTrigger"))
        {
            // Transmit input event to script in parent object
            switch (label)
            {
                case "BackSpace": 
                    parentKeyController.BackspacePressed(); 
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
            
            // Displace on click
            transform.localScale += new Vector3(0.01f, 0, 0.01f);
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("KeyboardTrigger"))
        {
            _renderer.material.color = defaultColor;
            
            // Displace on release
            transform.localScale = originalScale;
        }
    }
}
