using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    private SceneTransitionUIController sceneTransition;
    public void Init() 
    {
        sceneTransition = Managers.UI.MakeSceneUI<SceneTransitionUIController>(null,"SceneTransitionUI");
    }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();
        sceneTransition.StartCoroutine(LoadSceneCo(type));

    }
    private IEnumerator LoadSceneCo(Define.Scene type) 
    {
        float alpha = 0;
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            sceneTransition.canvasGroup.alpha =  Mathf.Lerp(alpha,1,progress);
            yield return null;
        }
        alpha = 1;
        sceneTransition.canvasGroup.alpha = alpha;
        SceneManager.LoadScene(GetSceneName(type));
        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            sceneTransition.canvasGroup.alpha = Mathf.Lerp(alpha, 0, progress);
            yield return null;
        }
        sceneTransition.canvasGroup.alpha = 0;
    }


    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene),type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
