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
    public void Init() 
    {
        sceneTransition = Managers.UI.MakeSceneUI<SceneTransitionUIController>(null,"SceneTransitionUI");
    }

    public void LoadScene(Define.Scene type ,Action onComplete = null)
    {
        Managers.Clear();
        sceneTransition.StartCoroutine(LoadSceneCo(GetSceneName(type), onComplete));
    }

    public void ReLoadCurrentScene(Action onComplete = null) 
    {
        Managers.Clear();
       
        sceneTransition.StartCoroutine(LoadSceneCo(SceneManager.GetActiveScene().name ,onComplete));
    }
    private IEnumerator LoadSceneCo(string sceneName, Action onComplete = null)
    {
        float alpha = 0;
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 1, progress);
            yield return null;
        }
        alpha = 1;
        sceneTransition.canvasGroup.alpha = alpha;
        var async =  SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        async.completed += (async) => { Managers.OnSceneLoad(); };
        
        while (async.progress < 0.9f)
        {
            progress += Time.deltaTime;
            sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 1, progress);
            yield return null;
        }
        DOTween.KillAll();
        async.allowSceneActivation = true;
        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 0, progress);
            yield return null;
        }
        sceneTransition.canvasGroup.alpha = 0;
       
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
