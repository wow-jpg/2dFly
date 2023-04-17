using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ.Input;

/// <summary>
/// 游戏结束界面控制
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    #region Fields
    [SerializeField] PlayerInput input;

    [SerializeField] Canvas HUDCanvas;

    [SerializeField] AudioData confirmGameOverSound;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");

    Canvas canvas;

    Animator animator;


    #endregion

    #region UnityCallBacks

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;
        input.onConfirmGameOver += OnConfirmGameOver;
    }

    

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;
        input.onConfirmGameOver -= OnConfirmGameOver;

    }



    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;

    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    #endregion


    #region Methods
    private void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisableAllInputs();
    }


    void EnableGameOverScreenInput()
    {
        input.EnableGameOverSceneInput();
    }

    private void OnConfirmGameOver()
    {

        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        input.DisableAllInputs();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();
    }
    #endregion
}
