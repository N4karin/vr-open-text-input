using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyFeedback : MonoBehaviour
{
    private KeyAudioFeedback soundHandler;
    private string label;
    private BoxCollider boxCol;
    private Vector3 originalBoxColSize;
    private KeyController parentKeyController;
    private bool isPressed;

    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
        boxCol = gameObject.GetComponent<BoxCollider>();
        originalBoxColSize = boxCol.size;
        parentKeyController = transform.parent.gameObject.GetComponent<KeyController>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger" && parentKeyController.getKeyReset())
        {
            // Block other keys from being pressed
            parentKeyController.setKeyReset(false);
            isPressed = true;
            
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
            // Allow other keys to be pressed again
            parentKeyController.setKeyReset(true);
            isPressed = false;
            
            // Scale box collider to match actual mesh again
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size = originalBoxColSize;
            // Displace on release
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0) ;
        }
    }
}
