using System;
using _panyaGame.Scripts.Platform_Related;
using UnityEngine;

namespace _panyaGame.Scripts.Player_Related
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpPower = 15f;
        [SerializeField] private float drag = 0.85f;
    
        [Header("References")]
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlatformGenerator platformGenerator;
    
        [Header("Lose Settings")]
        [SerializeField] private float fallThreshold = 6f;
        
        [Header("Screen Wrap Settings")]
        [SerializeField] private bool enableScreenWrap = true;
    
        private Rigidbody2D rb;
     
    
        private Camera mainCamera;
        private float screenHalfWidth;

        private bool isActive = true;

        public static event Action OnPlayerJumped;
        public static event Action OnPlayerLost;
    
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
        
            CalculateScreenBounds();
            
        }
    
        void Update()
        {
            // Platforma değince zıpla
            if (groundChecker.IsGrounded()  && rb.linearVelocity.y <= 0)
            {
                //Jump();
            }
            
            if (platformGenerator && transform.position.y < platformGenerator.LowestPlatformY - fallThreshold && isActive)
            {
                isActive = false;
                OnPlayerLost?.Invoke();
                Debug.Log("Player lost");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Obstacle"))
            {
                OnPlayerLost?.Invoke();
                isActive = false;
            }
        }

        void FixedUpdate()
        {
            if(!isActive) return;
            HandleMovement();
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
                    rb.linearVelocityX = direction * moveSpeed;
                }
                else if (!inputHandler.IsDragging())
                {
                    // Hiçbir input yoksa sürtünme uygula
                    rb.linearVelocityX *= drag;
                }
            }
        }

        public void Jump()
        {
            rb.linearVelocityY = jumpPower;
            OnPlayerJumped?.Invoke();
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
        
        public Vector2 GetBottomPoint() => groundChecker.transform.position;

        public Vector2 GetLinearVelocity()
        {
            return rb.linearVelocity;
        }
    }
}
