using UnityEngine;

namespace _panyaGame.Scripts.Player_Related
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private float dragSensitivity = 10f;
    
        private Camera mainCamera;
        private bool isDragging;
        private Vector2 dragStartPos;
        private float currentDirection; // -1, 0, veya 1
    
        void Start()
        {
            mainCamera = Camera.main;
        }
    
        void Update()
        {
            HandleInput();
        }
    
        void HandleInput()
        {
            currentDirection = 0f;
        
            // Klavye inputları
            float keyboardInput = GetKeyboardInput();
        
            // Mouse/Touch inputları
            float mouseInput = GetMouseInput();
        
            // Hangisi aktifse onu kullan (mouse/touch öncelikli)
            if (mouseInput != 0f)
            {
                currentDirection = mouseInput;
            }
            else if (keyboardInput != 0f)
            {
                currentDirection = keyboardInput;
            }
        }
    
        float GetKeyboardInput()
        {
            // Ok tuşları
            if (Input.GetKey(KeyCode.LeftArrow))
                return -1f;
            if (Input.GetKey(KeyCode.RightArrow))
                return 1f;
        
            // A-D tuşları
            if (Input.GetKey(KeyCode.A))
                return -1f;
            if (Input.GetKey(KeyCode.D))
                return 1f;
        
            return 0f;
        }
    
        float GetMouseInput()
        {
            // Mouse/Touch başlangıcı
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                dragStartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        
            // Mouse/Touch hareketi
            if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 currentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                float dragDelta = currentPos.x - dragStartPos.x;
            
                // Sürükleme yönüne göre direction (-1, 0, 1)
                float normalizedDrag = Mathf.Clamp(dragDelta * dragSensitivity, -1f, 1f);
                return normalizedDrag;
            }
        
            // Mouse/Touch bitişi
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        
            return 0f;
        }
    
        // Dışarıdan direction'ı almak için
        public float GetDirection()
        {
            return currentDirection;
        }
    
        public bool IsDragging()
        {
            return isDragging;
        }
    }
}
