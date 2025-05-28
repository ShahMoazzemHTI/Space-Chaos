using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] public float worldSpeed;
    [SerializeField] GameObject boss1;

    public int critterCount;





    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        critterCount = 0;
    }

    void Update()
    {
        if (critterCount > 10)
        {
            critterCount = 00;
            Instantiate(boss1, new Vector2(15f, 0), Quaternion.Euler(0f, 0f, -90f));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void GameOver()
    {
        StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("GameOver");

    }

}
