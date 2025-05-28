using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    //Energy
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text enegrySliderText;

    //Health
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthSliderText;

    //Pause 
    [SerializeField] GameObject pausePanel;



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
    }


    public void UpdateEnergySlider(float current, float max)
    {
        energySlider.value = current / max;
        string currentEnegryString = current.ToString("F0");
        enegrySliderText.text = currentEnegryString + " / " + max;
    }
    public void UpdateHealthSlider(float current, float max)
    {
        healthSlider.value = current / max;
        string currentHealthString = current.ToString("F0");
        healthSliderText.text = currentHealthString + " / " + max;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("Fire3"))
        {
            Pause();
        }
    }

    public void Pause()
    {

        if (pausePanel.activeSelf == false)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            PlayerController.Instance.OnApplicationPause(true);
            AudioManager.Instance.PlaySound(AudioManager.Instance.pause);
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            AudioManager.Instance.PlaySound(AudioManager.Instance.unpause);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
