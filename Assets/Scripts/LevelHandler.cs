using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] String scene_name;

    void Start()
    {
        Controls con = new Controls();
        con.GamePlay.Enable();
        con.GamePlay.Reset.performed += ResetLevel;
    }

    public void ResetLevel(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
    }

    public void AdvanceLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;
        // make sure we don't go out of bounds
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
        }
        SceneManager.LoadScene(nextIndex);
    }


}
