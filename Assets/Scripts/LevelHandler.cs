using System;
using UnityEditor.SearchService;
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
        //SceneManager.LoadScene()
    }


}
