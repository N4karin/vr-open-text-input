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
        originalScale = transform.localScale;
    }

    public void PressedFeedback()
    {
        // Change color 
        //_renderer.material.color = parentKeyController.getPressedColor();
            
        // Play sound
        soundHandler.PlayKeyClick();
            
        // Displace on click
        transform.localScale += new Vector3(0.02f, 0, 0.02f);
    }
    
    public void ReleasedFeedback()
    {
        //_renderer.material.color = parentKeyController.getDefaultColor();
            
        // Displace on release
        transform.localScale = originalScale;
    }
}
