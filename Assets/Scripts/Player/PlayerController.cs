using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 5f;
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        public Transform PivotTransform;
        private Vector3 _direction;

        public void Reset()
        {
            PivotTransform.rotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            transform.Translate(_direction * (speed * Time.deltaTime));
            animator.SetBool("isMoving", _direction != Vector3.zero);

            if (_direction.x > 0)
                PivotTransform.rotation = Quaternion.Euler(0, 0, 0);
            else if (_direction.x < 0)
                PivotTransform.rotation = Quaternion.Euler(0, 180, 0);
            else if (_direction.y > 0)
                PivotTransform.rotation = Quaternion.Euler(0, 0, 90);
            else if (_direction.y < 0)
                PivotTransform.rotation = Quaternion.Euler(0, 0, -90);
        }

        private void OnMove(InputValue value)
        {
            _direction = value.Get<Vector2>();
        }
    }
}
