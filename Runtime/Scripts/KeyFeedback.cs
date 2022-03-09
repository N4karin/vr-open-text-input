using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class KeyFeedback : MonoBehaviour
{
    [Serializable]
    public class TextInputEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class TextBackspaceEvent : UnityEvent
    {
    }
    
    private KeyAudioFeedback soundHandler;
    public bool keyHit = false;
    public bool keyReset = true;
    private string label;
    
    public Action onPress;

    private float originalY;
    private BoxCollider boxCol;
    
    public TextInputEvent onKeyPress;
    public TextBackspaceEvent onBackspace;
    
    void Start()
    {
        soundHandler = GameObject.FindGameObjectWithTag("SoundHandler").GetComponent<KeyAudioFeedback>();
        label = GetComponentInChildren<TextMeshPro>().text;
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
            boxCol = gameObject.GetComponent<BoxCollider>();
            boxCol.size += new Vector3(20, 100, 20);
            // Displace on click
            transform.position += transform.rotation * new Vector3(0, -0.005f, 0);
            // Transmit input event
            if (label == "BackSpace")
            {
                BackspacePressed();
            }
            else
            {
                KeyPressed(label);
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
            boxCol.size += new Vector3(-20, -100, -20);
            // Displace on release
            transform.position += transform.rotation * new Vector3(0, 0.005f, 0) ;
        }
    }
    
    private void KeyPressed(string text)
    {
        onKeyPress.Invoke(text);
    }

    private void BackspacePressed()
    {
        onBackspace.Invoke();
    }
}
