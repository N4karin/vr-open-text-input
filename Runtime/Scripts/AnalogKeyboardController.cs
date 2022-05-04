using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem.XR;
using UnityEngine.Events;
using TMPro;
using InputDevice = UnityEngine.XR.InputDevice;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class AnalogKeyboardController : MonoBehaviour
{
    [Serializable]
    public class StickTriggerUpdateEvent : UnityEvent
    {
    }
    
    [Serializable]
    public class TextInputEvent : UnityEvent<string>
    {
    }

    [Serializable]
    public class TextBackspaceEvent : UnityEvent
    {
    }
        
    [Serializable]
    public class TextReturnEvent : UnityEvent
    {
    }
    
    public Color defaultColor;
    public Color pressedColor;
    public bool rightHand = false;
    public bool invertModifierDirection = false;
    public TextInputEvent onKeyPress;
    public TextBackspaceEvent onBackspace;
    public TextReturnEvent onReturn;
    public StickTriggerUpdateEvent onStickTriggerMovement;

    private bool modifiedState = false;
    private bool bothModifiers = false;
    private bool triggerRight = false;
    private bool triggerLeft = false;
    private bool changeFromLastState = false;
    private bool keyPressed = false;
    private float triggerStateR = 0f;
    private float triggerStateL = 0f;
    private Vector2 _stickValue;
    private Vector3 baseLdefaultPosition;
    private Vector3 baseDefaultPosition;
    private Vector3 baseRdefaultPosition;
    private Vector3 baseLAndRdefaultPosition;
    private Vector3 defaultScale;
    private Transform _marker;
    private Transform activeDisk;
    private Transform lastTriggered;
    private Transform baseDefault;
    private Transform baseL;
    private Transform baseR;
    private Transform baseLAndR;
    private InputDevice controllerR;
    private InputDevice controllerL;
    private InputDevice activeController;

    // Start is called before the first frame update
    void Start()
    {
        activeDisk = transform.Find("Base");
        _marker = transform.Find("Marker");
        baseDefault = transform.Find("Base");
        baseL = transform.Find("Base L Modifier");
        baseR = transform.Find("Base R Modifier");
        baseLAndR = transform.Find("Base R + L Modifier");
        defaultScale = baseL.localScale;
        baseLdefaultPosition = baseL.localPosition;
        baseDefaultPosition = baseDefault.localPosition;
        baseRdefaultPosition = baseR.localPosition;
        baseLAndRdefaultPosition = baseLAndR.localPosition;
    
        // Invokes a function whenever the triggers and sticks are not in default position
        onStickTriggerMovement.AddListener(UpdateStickAndTriggerPosition);
    }

    void UpdateStickAndTriggerPosition()
    {
        // Move cursor
        _marker.transform.position = transform.position + transform.rotation * (new Vector3(_stickValue.x, 0 , _stickValue.y) * transform.localScale.magnitude * 0.13f);
        
        // Update trigger state variables
        bool tempL = triggerLeft;
        bool tempR = triggerRight;

        if (triggerStateR > 0.5)
        {
            triggerRight = true;
        }
        else
        {
            triggerRight = false;
        }
        
        if (triggerStateL > 0.5) triggerLeft = true;
        else triggerLeft = false;

        if (tempL != triggerLeft || tempR != triggerRight)
        {
            changeFromLastState = true;
            bothModifiers = false;
        }
        else
        {
            changeFromLastState = false;
        }
        
        // Displace disks when triggers released
        if ((!triggerLeft && !triggerRight && changeFromLastState) || (changeFromLastState && !bothModifiers)) 
        {
            modifiedState = false;
            bothModifiers = false;
            
            baseDefault.localScale = defaultScale + new Vector3(0.1f, 0, 0.1f);
        
            baseL.localScale = defaultScale;
            baseR.localScale = defaultScale;
            baseLAndR.localScale = defaultScale;
        
            baseDefault.transform.localPosition = baseDefaultPosition;
            baseL.transform.localPosition = baseLdefaultPosition;
            baseR.transform.localPosition = baseRdefaultPosition;
            baseLAndR.transform.localPosition = baseLAndRdefaultPosition;
            
            activeDisk = transform.Find("Base");
        }
        

        // Displace disks when both triggers pressed
        if (triggerLeft && triggerRight && changeFromLastState && !bothModifiers)
        {
            bothModifiers = true;
            modifiedState = true;
        
            baseDefault.localScale += new Vector3(-0.1f, 0, -0.1f);
            baseLAndR.localScale += new Vector3(0.1f, 0, 0.1f);
            
            baseDefault.transform.localPosition = baseDefaultPosition;
            baseL.transform.localPosition = baseLdefaultPosition;
            baseR.transform.localPosition = baseRdefaultPosition;
            baseLAndR.transform.localPosition = baseLAndRdefaultPosition;
            
            // Move everything down except cursor
            baseDefault.transform.localPosition += new Vector3(0, 0, -0.46f);
            baseL.transform.localPosition += new Vector3(0, 0, -0.46f);
            baseR.transform.localPosition += new Vector3(0, 0, -0.46f);
            baseLAndR.transform.localPosition += new Vector3(0, 0, -0.46f);
            
            activeDisk = transform.Find("Base R + L Modifier");
        }
        
        // Displace disks when only right trigger pressed
        else if (triggerRight && !modifiedState)
        {
            modifiedState = true;

            baseDefault.localScale += new Vector3(-0.1f, 0, -0.1f);
            baseR.localScale += new Vector3(0.1f, 0, 0.1f);
            
            // Move everything right except cursor
            baseDefault.transform.localPosition += new Vector3(-0.46f, 0, 0);
            baseL.transform.localPosition += new Vector3(-0.46f, 0, 0);
            baseR.transform.localPosition += new Vector3(-0.46f, 0, 0);
            baseLAndR.transform.localPosition += new Vector3(-0.46f, 0, 0);
            
            activeDisk = transform.Find("Base R Modifier");
        }
        
        // Displace disks when only left trigger pressed
        else if (triggerLeft && !modifiedState)
        {
            modifiedState = true;

            baseDefault.localScale += new Vector3(-0.1f, 0, -0.1f);
            baseL.localScale += new Vector3(0.1f, 0, 0.1f);
            
            // Move everything left except cursor
            baseDefault.transform.localPosition += new Vector3(0.46f, 0, 0);
            baseL.transform.localPosition += new Vector3(0.46f, 0, 0);
            baseR.transform.localPosition += new Vector3(0.46f, 0, 0);
            baseLAndR.transform.localPosition += new Vector3(0.46f, 0, 0);
            
            activeDisk = transform.Find("Base L Modifier");
        }
        
        // If stick is more than 50% in one direction, trigger key input based on cursor distance to next button
        if (_stickValue.magnitude >= 0.5 && !keyPressed)
        {
            string input = GetClosestButtonLabel(activeDisk);

            if (input == "Backspace")
            {
                BackspacePressed();
            }
            else if (input == "Enter")
            {
                ReturnPressed();
            } 
            else
            {
                KeyPressed(input);
            }
            
            keyPressed = true;
        }
        else if (_stickValue.magnitude < 0.5 && keyPressed)
        {
            lastTriggered.GetComponent<StickKeyFeedback>().ReleasedFeedback();
            keyPressed = false;
        }
    }
    
    // modified from https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    String GetClosestButtonLabel(Transform disk)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = _marker.transform.position;
        foreach (Transform t in disk) // Get all children of a Game Object and compare distance to cursor
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }

        lastTriggered = tMin;
        tMin.GetComponent<StickKeyFeedback>().PressedFeedback();
        return tMin.GetComponentInChildren<TextMeshPro>().text;
    }

    // Update is called once per frame
    void Update()
    {
        // Check whether trigger(s) pressed to enable modifiers
        controllerR.TryGetFeatureValue(CommonUsages.trigger, out triggerStateR);
        controllerL.TryGetFeatureValue(CommonUsages.trigger, out triggerStateL);
        activeController.TryGetFeatureValue(CommonUsages.primary2DAxis, out _stickValue);

        if (triggerStateR > 0 || triggerStateL > 0 || _stickValue.Equals(new Vector2(0f, 0f)))
        {
            onStickTriggerMovement.Invoke();
        }
    }

    private void FixedUpdate()
    {
        // Poll for connected XR controllers
        if (!controllerR.isValid)
        {
            var gameControllers = new List<InputDevice>();
            if (XRController.rightHand != null)
            {
                UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Right, gameControllers);
                controllerR = gameControllers[0];
                if (rightHand)
                {
                    activeController = controllerR;
                }
            }
        }
        
        if (!controllerL.isValid)
        {
            var gameControllers = new List<InputDevice>();
            if (XRController.leftHand != null)
            {
                UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left, gameControllers);
                controllerL = gameControllers[0];
                if (!rightHand)
                {
                    activeController = controllerL;
                }
            }
        }
    }
    
    public void KeyPressed(string text)
    {
        onKeyPress.Invoke(text);
    }

    public void BackspacePressed()
    {
        onBackspace.Invoke();
    }
        
    public void ReturnPressed()
    {
        onReturn.Invoke();
    }
    
    public Color getPressedColor()
    {
        return pressedColor;
    }
        
    public Color getDefaultColor()
    {
        return defaultColor;
    }
}
