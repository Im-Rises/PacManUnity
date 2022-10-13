using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public Animator anim;
    public GameObject[] hearts;
    public TextMeshProUGUI gameOverText;
    private int life;
    private Vector2 respawnPoint;


    private void Start()
    {
        life = hearts.Length;
        respawnPoint = transform.position;
        gameOverText.enabled = false;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Respawn()
    {
        anim.SetBool("isDead", false);
        transform.position = respawnPoint;
        GetComponent<PlayerInput>().enabled = true;
    }

    public void Die()
    {
        life--;
        GetComponent<PlayerInput>().enabled = false;
        hearts[life].SetActive(false);
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
}
