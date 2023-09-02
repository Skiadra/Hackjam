using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using static Movement;

public class GameManager : MonoBehaviour
{
    int index;
    public static bool loadStat;
    public static bool newStat;
    public static bool isMelee;
    public GameObject pauseMenu;
    public GameObject firstPauseButton;
    public GameObject guide;
    public GameObject typeSelectionUI;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void loadLevel()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        index = data.activeSceneIndex;
        loadStat = true;
        newStat = false;
        SceneManager.LoadSceneAsync(index);
        Destroy(GameObject.Find("Lighting"));
        Time.timeScale = 1f;
    }

    public void NewStart()
    {
        newStat = true;
        loadStat = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(GameObject.Find("Lighting"));
        Time.timeScale = 1f;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void guideMenuOn()
    {
        guide.SetActive(true);
    }

    public void TypeSelection()
    {
        typeSelectionUI.SetActive(true);
    }

    public void guideMenuOff()
    {
        guide.SetActive(false);
    }

    public void MeleeOn()
    {
        isMelee = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Boolean shooter false
    }

    public void ShooterOn()
    {
        isMelee = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //Boolean shooter true
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Time.timeScale = 0f;
            move.inControl = false;
            pauseMenu.SetActive(true);
            var eventSystem = EventSystem.current;
            eventSystem.SetSelectedGameObject(firstPauseButton, new BaseEventData(eventSystem));
        }
    }
}
