using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ö÷²Ëµ¥UI
/// </summary>
public class MainMenuUIController : MonoBehaviour
{
    [Header("=== Canvas ===")]
    [SerializeField] Canvas mainMenuCanvas;


    [Header("=== Buttons ===")]


    [SerializeField] Button buttonStartGame;
    [SerializeField] Button buttonOptions;
    [SerializeField] Button buttonQuit;


    private void OnEnable()
    {
        ButtonPressedBehaviour.buttonFunctions.Add(buttonStartGame.gameObject.name, OnStartGameButtonClick);
        ButtonPressedBehaviour.buttonFunctions.Add(buttonOptions.gameObject.name, OnButtonOptionsClicked);
        ButtonPressedBehaviour.buttonFunctions.Add(buttonQuit.gameObject.name, OnButtonQuitClicked);
    }

    private void OnDisable()
    {
        //  buttonStartGame.onClick.RemoveAllListeners();
        ButtonPressedBehaviour.buttonFunctions.Clear();
    }

    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStartGame);
    }

    void OnStartGameButtonClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGameplayScene();
    }

    void OnButtonOptionsClicked()
    {
        UIInput.Instance.SelectUI(buttonOptions);
    }

    void OnButtonQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = false;

#endif

        Application.Quit();
    }
}
