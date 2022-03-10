using System;
using System.Collections;
using System.Collections.Generic;
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
        if (collision.gameObject.tag == "KeyboardTrigger" && parentKeyController.getKeyReset())
        {
            // Block other keys from being pressed
            parentKeyController.setKeyReset(false);
            isPressed = true;
            // Change color 
            _renderer.material.color = pressedColor;
            // Play sound
            soundHandler.PlayKeyClick();
            // Scale box collider up to prevent multiple inputs
            boxCol.size += new Vector3(20, 100, 20);
            // Displace on click
            transform.position += transform.rotation * new Vector3(0, -0.005f, 0);
            // Transmit input event to script in parent object
            if (label == "BackSpace")
            { 
                parentKeyController.BackspacePressed();
            }
            else if (label == "Shift")
            {
                parentKeyController.setShiftPressed(true);
                parentKeyController.setKeyReset(true);
            }
            else
            {
                parentKeyController.KeyPressed(label);
            }
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger" && isPressed)
        {
            if (label == "Shift")
            {
                parentKeyController.setShiftPressed(false);
            }

            // Allow other keys to be pressed again
            parentKeyController.setKeyReset(true);
            isPressed = false;
            _renderer.material.color = defaultColor;
            
            // Scale box collider to match actual mesh again
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size = originalBoxColSize;
            // Displace on release
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0) ;
        }
    }
}
