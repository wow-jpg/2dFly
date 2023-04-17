using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSigleton<SceneLoader>
{
    [SerializeField] Image transitionImage;
    [SerializeField] float fadeTime = 3.5f;

    Color color;

    const string GAME = "Game";
    const string MAIN_MENU = "MainMenu";
    const string SCORING = "Scoring";

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }



    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation=SceneManager.LoadSceneAsync(sceneName);

        loadingOperation.allowSceneActivation = false;

        transitionImage.gameObject.SetActive(true);

        while (color.a < 1f)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }

       yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        //   Load(sceneName);
        loadingOperation.allowSceneActivation = true;

        while (color.a > 0f)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }


        transitionImage.gameObject.SetActive(false);
    }

    public void LoadGameplayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GAME));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MAIN_MENU));
    }


    public void LoadScoringScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(SCORING));
    }

    



}
