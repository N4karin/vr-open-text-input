using System;
using UnityEngine;
using UnityEngine.Events;

public class DrumKeyController : MonoBehaviour
    { 
        [Serializable]
        public class TextInputEvent : UnityEvent<string>
        {
        }

        [Serializable]
        public class TextBackspaceEvent : UnityEvent
        {
        }
        
        public Color defaultColor;
        public Color pressedColor;
        public TextInputEvent onKeyPress;
        public TextBackspaceEvent onBackspace;
        private bool _keyReset;

        private void Start()
        {
            _keyReset = true;
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
            return _keyReset;
        }

        public void setKeyReset(bool isNoKeyPressed)
        {
            _keyReset = isNoKeyPressed;
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