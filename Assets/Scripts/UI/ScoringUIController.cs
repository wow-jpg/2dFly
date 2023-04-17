using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// ����UI������
/// </summary>
public class ScoringUIController : MonoBehaviour
{
    #region Fields

    [Header("=== ���� ===")]
    [SerializeField] Image background;

    [SerializeField] Sprite[] backgroundImage;

    [Header("=== ���� ===")]

    [SerializeField] Canvas scoringScreenCanvas;
    [SerializeField] Text playerScoreText;
    [SerializeField] Button buttonMainMenu;
    [SerializeField] Transform highScoreLeaderboardContainer;


    [Header("=== ����߷�ʱ�Ľ��� ===")]
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
    /// ��ʾ��¼�����Ľ���
    /// </summary>
    private void ShowNewHighScoreScreen()
    {
        newHighScoreCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);
    }

    /// <summary>
    /// ���ؼ�¼�����Ľ���
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
    /// �������
    /// </summary>
    void ShowRandomBackground()
    {
        background.sprite = backgroundImage[Random.Range(0, backgroundImage.Length)];    
    }
	
    /// <summary>
    /// ��ʾ��ҵ÷�
    /// </summary>
    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);

        UpdateHighScoreLeaderboard();

    }


    /// <summary>
    /// ���¸߷ְ�
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
    /// ���²˵���ť��ִ�еķ���
    /// </summary>
    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    /// <summary>
    /// ������ύ��ťʱ
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
 