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

		[Serializable]
        public class TextReturnEvent : UnityEvent
        {
        }
        
        public Color defaultColor;
        public Color pressedColor;
        public TextInputEvent onKeyPress;
        public TextBackspaceEvent onBackspace;
		public TextReturnEvent onReturn;
        
		private bool _shiftPressed;
        private bool _keyReset;

        private void Start()
        {
            _keyReset = true;
        }

        public void KeyPressed(string text)
        {
            if (_shiftPressed)
            {
                text = text.ToUpper();
            }
            else
            {
                text = text.ToLower();
            }
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

		public bool getShiftPressed()
        {
            return _shiftPressed;
        }

        public void setShiftPressed(bool isShiftPressed)
        {
            _shiftPressed = isShiftPressed;
        }
}