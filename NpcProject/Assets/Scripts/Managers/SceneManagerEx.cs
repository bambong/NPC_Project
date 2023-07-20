using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }


    private SceneTransitionUIController sceneTransition;
    public Action OnSceneUnload;
    private bool isTransitioning;
    public bool IsTransitioning { get => isTransitioning;}
    private const float TRANSITION_ANIM_TIME = 1f;
    public void Init() 
    {
        sceneTransition = Managers.UI.MakeSceneUI<SceneTransitionUIController>(null,"SceneTransitionUI");
    }

    public void LoadScene(Define.Scene type ,Action onComplete = null)
    {
        sceneTransition.StartCoroutine(LoadSceneCo(GetSceneName(type), onComplete));
    }
    public void LoadScene(string sceneName, Action onComplete = null)
    {
        sceneTransition.StartCoroutine(LoadSceneCo(sceneName, onComplete));
    }

    public void ReLoadCurrentScene(Action onComplete = null) 
    {
        sceneTransition.StartCoroutine(LoadSceneCo(SceneManager.GetActiveScene().name ,onComplete,false));
    }
    private IEnumerator LoadSceneCo(string sceneName, Action onComplete = null, bool isSave = true)
    {
        isTransitioning = true;
        string prevSceneName = SceneManager.GetActiveScene().name;
   

        sceneTransition.OpenPanel(TRANSITION_ANIM_TIME);
        sceneTransition.SetPercentUpdate(0);
        yield return new WaitForSeconds(TRANSITION_ANIM_TIME);

        var async =  SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        async.completed += (async) => { Managers.OnSceneLoad(); };
        while ( async.progress < 0.9f)
        {
            sceneTransition.SetPercentUpdate((int)(async.progress * 100));
           // sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 1, progress);
            yield return null;
        }
        /// 데이터 세이브 
        if (isSave) 
        {
            Managers.Data.SaveGame();
        }

       //sceneTransition.canvasGroup.alpha = 1;
         Managers.Clear();
        DOTween.KillAll();
        async.allowSceneActivation = true;
        while (!async.isDone)
        {
            sceneTransition.SetPercentUpdate((int)(async.progress * 100));
            yield return null;
        }
        
        sceneTransition.SetPercentUpdate(100);
        sceneTransition.ClosePanel(TRANSITION_ANIM_TIME);
        yield return new WaitForSeconds(TRANSITION_ANIM_TIME);

        isTransitioning = false;
        onComplete?.Invoke();
    }


    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene),type);
        return name;
    }

    public void Clear()
    {
        OnSceneUnload?.Invoke();
        OnSceneUnload = null;
        CurrentScene.Clear();
    }
}
