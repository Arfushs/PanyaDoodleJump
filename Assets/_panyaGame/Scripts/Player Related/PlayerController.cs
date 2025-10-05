using System;
using UnityEngine;

namespace _panyaGame.Scripts.Player_Related
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpPower = 15f;
        [SerializeField] private float gravity = 20f;
        [SerializeField] private float drag = 0.85f;
    
        [Header("References")]
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private InputHandler inputHandler;
    
        [Header("Screen Wrap Settings")]
        [SerializeField] private bool enableScreenWrap = true;
    
        private Rigidbody2D rb;
        private Vector2 velocity;
        private bool wasGrounded;
    
        private Camera mainCamera;
        private float screenHalfWidth;

        public static event Action OnPlayerJumped;
    
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            mainCamera = Camera.main;
        
            CalculateScreenBounds();
            
        }
    
        void Update()
        {
            // Platforma değince zıpla
            if (groundChecker && groundChecker.IsGrounded() && !wasGrounded && velocity.y <= 0)
            {
                Jump();
            }
        
            wasGrounded = groundChecker != null ? groundChecker.IsGrounded() : false;
        }
    
        void FixedUpdate()
        {
            HandleMovement();
            ApplyPhysics();
            ApplyMovement();
            CheckScreenWrap();
        }
    
        void CalculateScreenBounds()
        {
            if (mainCamera != null)
            {
                screenHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            }
        }
    
        // Input'a göre hareketi ayarla
        void HandleMovement()
        {
            if (inputHandler)
            {
                float direction = inputHandler.GetDirection();
            
                if (direction != 0f)
                {
                    // Direction -1, 0 veya 1 değerinde
                    velocity.x = direction * moveSpeed;
                }
                else if (!inputHandler.IsDragging())
                {
                    // Hiçbir input yoksa sürtünme uygula
                    velocity.x *= drag;
                }
            }
        }
    
        void ApplyPhysics()
        {
            // Manuel yerçekimi
            if (!groundChecker || !groundChecker.IsGrounded())
            {
                velocity.y -= gravity * Time.fixedDeltaTime;
            }
        }
    
        void Jump()
        {
            velocity.y = jumpPower;
            OnPlayerJumped?.Invoke();
        }
    
        void ApplyMovement()
        {
            rb.linearVelocity = velocity;
        }
    
        void CheckScreenWrap()
        {
            if (!enableScreenWrap) return;
        
            Vector3 pos = transform.position;
        
            if (pos.x < -screenHalfWidth)
            {
                pos.x = screenHalfWidth;
                transform.position = pos;
            }
            else if (pos.x > screenHalfWidth)
            {
                pos.x = -screenHalfWidth;
                transform.position = pos;
            }
        }
    
        // Debug bilgileri
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 14;
            style.normal.textColor = Color.white;
        
            GUI.Label(new Rect(10, 10, 300, 20), "Kontroller: Ok Tuşları / A-D / Mouse Sürükle", style);
            GUI.Label(new Rect(10, 30, 300, 20), $"Hız: X:{velocity.x:F1} Y:{velocity.y:F1}", style);
        
            if (groundChecker)
            {
                GUI.Label(new Rect(10, 50, 300, 20), $"Zemin: {(groundChecker.IsGrounded() ? "Evet" : "Hayır")}", style);
            }
        
            if (inputHandler)
            {
                float dir = inputHandler.GetDirection();
                string dirText = dir > 0 ? "Sağ (1)" : dir < 0 ? "Sol (-1)" : "Yok (0)";
                GUI.Label(new Rect(10, 70, 300, 20), $"Direction: {dirText}", style);
            }
        }
    }
}
