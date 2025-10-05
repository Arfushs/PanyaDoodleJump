using UnityEngine;

namespace _panyaGame.Scripts.Player_Related 
{     
    public class PlayerAnimController : MonoBehaviour     
    {         
        [SerializeField] private Animator animator;         
        [SerializeField] private InputHandler inputHandler;          
        
        private void OnEnable()         
        {             
            PlayerController.OnPlayerJumped += OnPlayerJumped;         
        }                  
        
        private void OnDisable()         
        {             
            PlayerController.OnPlayerJumped -= OnPlayerJumped;         
        }                  
        
        private void OnPlayerJumped()         
        {             
            animator.SetTrigger("OnJumped");     
        }          
        
        private void Update()         
        {             
            SetCharacterRotation();         
        }
        
        private void SetCharacterRotation()
        {
            if (!inputHandler) return;
            
            float direction = inputHandler.GetDirection();
            
            // Only rotate if there's actual input
            if (direction != 0f)
            {
                // Get current rotation
                Vector3 rotation = transform.eulerAngles;
                
                // direction > 0 means right, direction < 0 means left
                rotation.y = direction > 0 ? 0f : 180f;
                
                // Apply rotation
                transform.eulerAngles = rotation;
            }
        }
    } 
}