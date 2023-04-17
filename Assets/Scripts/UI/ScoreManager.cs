using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ.System;

public class ScoreManager : PersistentSigleton<ScoreManager>
{
    public int Score => score;
    int score;
    int currentScore;

    Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    const string saveFileName = "player_score.json";

    string playerName = "No Name";

    /// <summary>
    /// �Ƿ���ڵ�ʮλ�ĵ÷�
    /// </summary>
    public bool hasNewHighScore => score > LoadPlayerScoreData().list[9].score;

    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }


    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }


    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);
        while (score < currentScore)
        {
            score++;
            ScoreDisplay.UpdateText(score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }



    #region �߷ְ�ϵͳ

    [System.Serializable]
    public class PlayerScore
    {
        public int score;

        public string playerName;

        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }
    }

    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> list = new List<PlayerScore>();
    }

    /// <summary>
    /// ��ȡ��ҵ÷�����
    /// </summary>
    /// <returns></returns>
    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();
        //TODO:��û���

        if (SaveSystem.SaveFileExists(saveFileName))
        {
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(saveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < 10)
            {
                playerScoreData.list.Add(new PlayerScore(0, playerName));

            }

            SaveSystem.SaveByJson(saveFileName, playerScoreData);
        }

        return playerScoreData;

    }


    /// <summary>
    /// ������ҵ�����
    /// </summary>
   public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();
        playerScoreData.list.Add(new PlayerScore(score, playerName));
        playerScoreData.list.Sort((x, y) => y.score.CompareTo(x.score));

        SaveSystem.SaveByJson(saveFileName, playerScoreData);
    }

    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    #endregion




}
