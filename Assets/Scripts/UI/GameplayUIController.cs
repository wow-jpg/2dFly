using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZJ.Input;

public class GameplayUIController : MonoBehaviour
{

    [SerializeField] PlayerInput playerInput;
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("----Player Input----")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button mainMenuButton;
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;


    int buttonPressedParameterID = Animator.StringToHash("Pressed");
    void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;


        ButtonPressedBehaviour.buttonFunctions.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        ButtonPressedBehaviour.buttonFunctions.Add(optionsButton.gameObject.name, OnOptionsButtonClick);
        ButtonPressedBehaviour.buttonFunctions.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);

    }

    void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        ButtonPressedBehaviour.buttonFunctions.Clear();
    }

    void Start()
    {
        menusCanvas.enabled = false;


    }


    public void Unpause()
    {
        resumeButton.Select();
        OnResumeButtonClick();
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    public void Pause()
    {
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnablePauseMenuInput();
        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    /// <summary>
    /// 恢复游戏按钮
    /// </summary>
    void OnResumeButtonClick()
    {
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        playerInput.EnableGamePlayInput();
    }

    /// <summary>
    /// 选项按钮
    /// </summary>
    void OnOptionsButtonClick()
    {
        //TODO:
        UIInput.Instance.SelectUI(optionsButton);
        playerInput.EnablePauseMenuInput();
    }

    /// <summary>
    /// 回到菜单按钮
    /// </summary>
    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}
