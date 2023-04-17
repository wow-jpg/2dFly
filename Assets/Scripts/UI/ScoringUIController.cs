using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 分数UI控制器
/// </summary>
public class ScoringUIController : MonoBehaviour
{
    #region Fields

    [Header("=== 背景 ===")]
    [SerializeField] Image background;

    [SerializeField] Sprite[] backgroundImage;

    [Header("=== 分数 ===")]

    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;
    [SerializeField] Transform highScoreLeaderboardContainer;


    [Header("=== 打出高分时的界面 ===")]
    [SerializeField] Canvas newHighScoreCanvas;
    [SerializeField] Button buttonCancel;
    [SerializeField] Button buttonSubmit;
    [SerializeField] InputField playerNameInputField;
    #endregion

	#region UnityCallBacks
	
    void Start()
    {

        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowRandomBackground();

        if(ScoreManager.Instance.hasNewHighScore)
        {

            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen();

        }



        ButtonPressedBehaviour.buttonFunctions.Add(buttonMainMenu.gameObject.name, OnButtonMainMenuClicked);
        ButtonPressedBehaviour.buttonFunctions.Add(buttonSubmit.gameObject.name, OnButtonSubmitClicked);
        ButtonPressedBehaviour.buttonFunctions.Add(buttonCancel.gameObject.name, HideNewHighScoreScreen);

        GameManager.GameState = GameState.Scoring;

    }

    /// <summary>
    /// 显示记录分数的界面
    /// </summary>
    private void ShowNewHighScoreScreen()
    {
        newHighScoreCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);
    }

    /// <summary>
    /// 隐藏记录分数的界面
    /// </summary>
    private void HideNewHighScoreScreen()
    {
        newHighScoreCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour.buttonFunctions.Clear();
    }

    void Update()
    {
        
    }

	#endregion


	#region Methods

    /// <summary>
    /// 随机背景
    /// </summary>
    void ShowRandomBackground()
    {
        background.sprite = backgroundImage[Random.Range(0, backgroundImage.Length)];    
    }
	
    /// <summary>
    /// 显示玩家得分
    /// </summary>
    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);

        UpdateHighScoreLeaderboard();

    }


    /// <summary>
    /// 更新高分榜
    /// </summary>
    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;

        }
    }


    /// <summary>
    /// 按下菜单按钮后执行的方法
    /// </summary>
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    /// <summary>
    /// 当点击提交按钮时
    /// </summary>
    void OnButtonSubmitClicked()
    {
        if(!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);


        }


        HideNewHighScoreScreen();
    }


    #endregion
}
 