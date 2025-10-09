using UnityEngine;

namespace _panyaGame.Scripts.Player_Related
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Ground Check Settings")]
        [SerializeField] private Vector2 checkSize = new Vector2(0.4f, 0.1f);
        [SerializeField] private LayerMask platformLayer;
        [SerializeField] private bool showDebugGizmo = true;

        private bool isGrounded;

        void FixedUpdate()
        {
            CheckGround();
        }

        void CheckGround()
        {
            // Box check instead of circle
            isGrounded = Physics2D.OverlapBox(transform.position, checkSize, 0f, platformLayer);
        }

        public bool IsGrounded()
        {
            return isGrounded;
        }

        private void OnDrawGizmos()
        {
            if (!showDebugGizmo) return;

            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(transform.position, checkSize);
        }
    }
}