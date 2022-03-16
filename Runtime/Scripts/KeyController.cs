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
        
        [Serializable]
        public class TextReturnEvent : UnityEvent
        {
        }
        
        [Serializable]
        public class TextArrowEvent : UnityEvent
        {
        }
        
        [Serializable]
        public class TextTabulatorEvent : UnityEvent
        {
        }
        
        public Color defaultColor;
        public Color pressedColor;
        public TextInputEvent onKeyPress;
        public TextBackspaceEvent onBackspace;
        public TextReturnEvent onReturn;
        public TextArrowEvent onLeftArrow;
        public TextArrowEvent onRightArrow;
        public TextTabulatorEvent onTabulator;
		private bool _shiftPressed;
        private bool _keyReset;
        private bool _capsLock;
        private GameObject _capsLockIndicator;

        private void Start()
        {
            _keyReset = true;
            _capsLockIndicator = GameObject.Find("Caps Lock Indicator");
            _capsLockIndicator.SetActive(false);
        }

        public void KeyPressed(string text)
        {
            // Is a letter
            if (text.Length == 1)
            {
                if (_shiftPressed || _capsLock)
                {
                    text = text.ToUpper();
                }
                else
                {
                    text = text.ToLower();
                }
            }
            // Is number or symbol
            else
            {
                if (!_shiftPressed)
                {
                    text = text[0].ToString();
                }
                else
                {
                    text = text[2].ToString();
                }
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
        
        public void RightArrowPressed()
        {
            onRightArrow.Invoke();
        }
        
        public void LeftArrowPressed()
        {
            onLeftArrow.Invoke();
        }
        
        public void TabulatorPressed()
        {
            onTabulator.Invoke();
        }
        

        public bool getKeyReset()
        {
            return _keyReset;
        }
        
        public void ToggleCaps()
        {
            if (_capsLock)
            {
                _capsLockIndicator.SetActive(false);
                _capsLock = false;
            }
            else
            {
                _capsLockIndicator.SetActive(true);
                _capsLock = true;
            }
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