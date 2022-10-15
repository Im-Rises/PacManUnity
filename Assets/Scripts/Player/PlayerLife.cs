using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [Header("Player Life")] public int life = 3;

    [Header("Animation")] public Animator anim;

    [Header("Text UI")] public TextMeshProUGUI gameOverText;

    [Header("Point of life")] public Sprite heartTexture;

    public GameObject heartsParent;
    public float hearthOffset = 1.5f;
    private GameObject[] hearts;
    private Vector2 respawnPoint;


    private void Start()
    {
        respawnPoint = transform.position;
        gameOverText.enabled = false;
        hearts = new GameObject[life];

        for (var i = 0; i < life; i++)
        {
            var heart = new GameObject();
            heart.AddComponent<SpriteRenderer>().sprite = heartTexture;
            heart.transform.position =
                new Vector2(heartsParent.transform.position.x + i * hearthOffset, heartsParent.transform.position.y);
            hearts[i] = heart;
        }
    }


    private void Respawn()
    {
        // GetComponent<PlayerController>().Reset();
        anim.SetBool("isDead", false);
        transform.position = respawnPoint;
        GetComponent<PlayerInput>().enabled = true;
        hearts[life].SetActive(false);
    }

    public void Die()
    {
        life--;
        GetComponent<PlayerInput>().enabled = false;
        anim.SetBool("isDead", true);
        if (life <= 0)
        {
            gameOverText.enabled = true;
            Invoke("ReloadScene", 4f);
        }
        else
        {
            Invoke("Respawn", 2f);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
