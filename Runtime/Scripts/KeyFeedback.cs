using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyFeedback : MonoBehaviour
{
    private KeyAudioFeedback soundHandler;
    public bool keyHit = false;
    public bool keyReset = true;
    private string label;

    private BoxCollider boxCol;
    private Vector3 originalBoxColSize;

    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
        boxCol = gameObject.GetComponent<BoxCollider>();
        originalBoxColSize = boxCol.size;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger" && keyReset)
        {
            soundHandler.PlayKeyClick();
            Debug.Log("Pressed a key");
            keyHit = true;
            keyReset = false;
            // Scale box collider up to prevent multiple inputs
            boxCol.size += new Vector3(20, 100, 20);
            // Displace on click
            transform.position += transform.rotation * new Vector3(0, -0.005f, 0);
            // Transmit input event
            if (label == "BackSpace")
            {
                transform.parent.gameObject.GetComponent<KeyController>().BackspacePressed();
            }
            else
            {
                transform.parent.gameObject.GetComponent<KeyController>().KeyPressed(label);
            }
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger" && keyHit)
        {
            Debug.Log("Released a key");
            keyHit = false;
            keyReset = true;
            // Scale box collider to match actual mesh again
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size = originalBoxColSize;
            // Displace on release
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0) ;
        }
    }
}
