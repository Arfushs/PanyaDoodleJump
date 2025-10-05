using UnityEngine;

namespace _panyaGame.Scripts.Player_Related
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Ground Check Settings")]
        [SerializeField] private float checkRadius = 0.2f;
        [SerializeField] private LayerMask platformLayer;
        [SerializeField] private bool showDebugGizmo = true;
    
        private bool isGrounded;
    
        void FixedUpdate()
        {
            CheckGround();
        }
    
        void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, platformLayer);
        }
    
        public bool IsGrounded()
        {
            return isGrounded;
        }
    
        // Unity Editor'de görsel olarak görmek için
        void OnDrawGizmos()
        {
            if (showDebugGizmo)
            {
                Gizmos.color = isGrounded ? Color.green : Color.yellow;
                Gizmos.DrawWireSphere(transform.position, checkRadius);
            }
        }
    }
}