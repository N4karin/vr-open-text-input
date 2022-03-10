using System;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class KeyController : MonoBehaviour
    { 
        [Serializable]
        public class TextInputEvent : UnityEvent<string>
        {
        }

        [Serializable]
        public class TextBackspaceEvent : UnityEvent
        {
        }
        
        public TextInputEvent onKeyPress;
        public TextBackspaceEvent onBackspace;
        private TextMeshPro _tmp;
        private bool keyReset;
        public Color defaultColor;
        public Color pressedColor;

        private void Start()
        {
            keyReset = true;
        }

        public void KeyPressed(string text)
        {
            onKeyPress.Invoke(text);
        }

        public void BackspacePressed()
        {
            onBackspace.Invoke();
        }

        public bool getKeyReset()
        {
            return keyReset;
        }

        public void setKeyReset(bool isNoKeyPressed)
        {
            keyReset = isNoKeyPressed;
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