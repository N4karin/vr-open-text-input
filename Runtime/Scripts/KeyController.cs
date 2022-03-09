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
        public Color defaultColor;
        public Color pressedColor;

        private TextMeshPro _tmp;
        private Renderer _renderer;
        private bool keyReset;

        private void Start()
        {
            _tmp = GetComponentInChildren<TextMeshPro>();
            _renderer = GetComponent<Renderer>();
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
}