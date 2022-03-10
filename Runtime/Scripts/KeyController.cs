using System;
using UnityEngine;
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
        
        public Color defaultColor;
        public Color pressedColor;
        public TextInputEvent onKeyPress;
        public TextBackspaceEvent onBackspace;
		private bool shiftPressed;
        private bool keyReset;

        private void Start()
        {
            keyReset = true;
        }

        public void KeyPressed(string text)
        {
			if (!shiftPressed)
			{
				text = text.ToLower();
			}
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

		public bool getShiftPressed()
        {
            return shiftPressed;
        }

        public void setShiftPressed(bool isShiftPressed)
        {
            shiftPressed = isShiftPressed;
        }
}