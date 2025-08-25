using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class LevelScreens : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject levelEndScreenPrefab;
    [SerializeField] string lossSplashText;
    [SerializeField] string lossButtonText;
    UnityAction menuButtonAction;
    UnityAction nextButtonAction;
    LevelHandler levelHandler;

    Controls con;
    void Start()
    {
        levelHandler = GameObject.FindAnyObjectByType<LevelHandler>();
        con = new Controls();
        con.GamePlay.Enable();
        con.GamePlay.Pause.performed += TogglePauseScreen;
        con.PauseMenu.Unpause.performed += TogglePauseScreen;

        pauseScreen.SetActive(false);
    }

    public void TogglePauseScreen(InputAction.CallbackContext context)
    {
        if (pauseScreen.activeInHierarchy)
        {
            con.PauseMenu.Disable();
            con.GamePlay.Enable();
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
        else
        {
            con.PauseMenu.Enable();
            con.GamePlay.Disable();
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
    }

    public void EndLevel(bool win)
    {
        GameObject endScreen = Instantiate(levelEndScreenPrefab, transform);
        Button nextLevelButton = endScreen.transform.Find("NextLevel").gameObject.GetComponent<Button>();
        Button menuButton = endScreen.transform.Find("ReturnToMenu").gameObject.GetComponent<Button>();

        // print(nextLevelButton);
        // print(menuButton);

        menuButtonAction += levelHandler.LoadMenu;
        menuButton.onClick.AddListener(menuButtonAction);
        if (!win)
        {
            // change the text
            endScreen.transform.Find("FinishText").gameObject.GetComponent<TextMeshProUGUI>()
                .text = lossSplashText;
            // change the button text and listener

            endScreen.transform.Find("NextLevel").gameObject.GetComponent<TextMeshProUGUI>()
                .text = lossButtonText;
            nextButtonAction += levelHandler.ResetLevel;
            nextLevelButton.onClick.AddListener(nextButtonAction);
        }
        else
        {
            nextButtonAction += levelHandler.AdvanceLevel;
            nextLevelButton.onClick.AddListener(nextButtonAction);
        }
        endScreen.SetActive(true);
    }
}
