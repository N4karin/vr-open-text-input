using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFeedback : MonoBehaviour
{
    private KeyAudioFeedback soundHandler;
    public bool keyHit = false;
    public bool keyReset = false;

    private float originalY;
    private BoxCollider boxCol;
    
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        originalY = transform.position.y;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger")
        {
            soundHandler.PlayKeyClick();
            Debug.Log("Pressed a key");
            keyHit = true;
            keyReset = false;
            // Displace Box Collider to prevent multiple inputs
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size += new Vector3(20, 100, 20);
            // Displace on click
            transform.position += transform.rotation * new Vector3(0, -0.005f, 0);
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "KeyboardTrigger")
        {
            Debug.Log("Released a key");
            keyHit = false;
            keyReset = true;
            // Displace Box Collider to prevent multiple inputs
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size += new Vector3(-20, -100, -20);
            // Displace on release
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0) ;
        }
    }
}
