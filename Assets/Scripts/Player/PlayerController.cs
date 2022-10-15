using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public float speed = 10f;
        public Tilemap tilemap;
        public Vector2 _orignalDirection = new(-1, 0);
        public float initPositionOffset = 0.5f;

        private Vector2 _destination;
        private Vector2 _inputDirection;
        private Vector2 _lastDirection;
        private Vector2 _lastInput;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _destination = (Vector2)transform.position + _orignalDirection * initPositionOffset;
            _lastDirection = _orignalDirection;
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            var positionVector = Vector2.MoveTowards(position, _destination, speed * Time.deltaTime);
            _rigidbody2D.MovePosition(positionVector);

            var isCentered = (Vector2)position == _destination;

            if (!isCentered) return;

            // if is at the middle of a tile, has input and there is no wall in the direction of the input
            if (_lastInput != Vector2.zero && !DetectWallBorder(_lastInput))
            {
                _destination = (Vector2)transform.position + _lastInput;
                _lastDirection = _lastInput;
            }
            // if is at the middle of a tile and there is no wall in the current direction then continue in the same direction
            else if (!DetectWallBorder(_lastDirection))
            {
                _destination = (Vector2)transform.position + _lastDirection;
            }
        }

        private bool DetectWallBorder(Vector3 dir)
        {
            var pos = transform.position;
            var cellPosition = tilemap.WorldToCell(pos + dir);
            var linecast = Physics2D.Linecast(pos + dir, pos);
            return tilemap.HasTile(cellPosition) || linecast.collider.CompareTag(tilemap.tag);
        }

        private void OnMove(InputValue value)
        {
            _inputDirection = value.Get<Vector2>();
            if (_inputDirection != Vector2.zero) _lastInput = _inputDirection;
        }
    }
}
