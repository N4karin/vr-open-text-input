using System;
using TMPro;
using UnityEngine;
using UnityEditor;

public class KeyController : MonoBehaviour
    {
        public Color defaultColor;
        public Color pressedColor;
        

        private TextMeshPro _tmp;
        private Renderer _renderer;

        private void Start()
        {
            _tmp = GetComponentInChildren<TextMeshPro>();
            _renderer = GetComponent<Renderer>();
        }

        public void Update()
        {
            
        }
        
        void OnCollisionEnter(Collision collision)
        {
            
        }
}