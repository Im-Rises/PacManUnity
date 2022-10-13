using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed = 3f;
    public Transform[] waypoints;
    public SpriteRenderer eyesSpriteRenderer;

    public Sprite[] eyesSpriteArray;

    // private Vector2 _direction;
    private int cur;

    private void FixedUpdate()
    {
        if (transform.position != waypoints[cur].position)
        {
            var p = Vector2.MoveTowards(transform.position,
                waypoints[cur].position,
                speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }

        // if (_direction.x > 0)
        //     eyesSpriteRenderer.sprite = eyesSpriteArray[0];
        // else if (_direction.x < 0)
        //     eyesSpriteRenderer.sprite = eyesSpriteArray[1];
        // else if (_direction.y > 0)
        //     eyesSpriteRenderer.sprite = eyesSpriteArray[2];
        // else if (_direction.y < 0)
        //     eyesSpriteRenderer.sprite = eyesSpriteArray[3];

        var dir = waypoints[cur].position - transform.position;
        if (dir.x > 0)
            eyesSpriteRenderer.sprite = eyesSpriteArray[0];
        if (dir.x < 0)
            eyesSpriteRenderer.sprite = eyesSpriteArray[1];
        if (dir.y > 0)
            eyesSpriteRenderer.sprite = eyesSpriteArray[2];
        if (dir.y < 0)
            eyesSpriteRenderer.sprite = eyesSpriteArray[3];
    }

    private void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name == "Player")
            co.GetComponent<PlayerLife>().Die();
    }
}
