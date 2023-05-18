using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerroomPuzzleController : MonoBehaviour ,IInteraction
{
    [SerializeField]
    private PuzzleMatchCheckController[] puzzleMatchChecks;

    [SerializeField]
    private int clearTalk;

    [SerializeField]
    private int nonClearTalk;

    [SerializeField]
    private Material serverrackMaterial;

    [SerializeField]
    private Texture serverrackOnTex; 
    [SerializeField]
    private Texture serverrackOffTex;

    [SerializeField]
    private GameObject potal;

    //[SerializeField]
    //private AudioClip lightOnSound;

    public GameObject Go => gameObject;

    public bool IsInteractAble => true;

    private bool isClear = false;

    private void Start()
    {
        serverrackMaterial.mainTexture = serverrackOffTex;
        serverrackMaterial.DisableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();
    }
    public void OnInteraction()
    {
        if (isClear) 
        {
            return;
        }
        foreach(var puzzle in puzzleMatchChecks) 
        {
            if (!puzzle.IsClear()) 
            {

 
                Managers.Talk.PlayCurrentSceneTalk(nonClearTalk);
                return;
            }
        }

        isClear = true;
        foreach (var puzzle in puzzleMatchChecks)
        {
            puzzle.Clear();
        }
        serverrackMaterial.mainTexture = serverrackOnTex;
        serverrackMaterial.EnableKeyword("_EMISSION");
        DynamicGI.UpdateEnvironment();

        Managers.Talk.PlayCurrentSceneTalk(clearTalk);
        //Managers.Sound.AskSfxPlay(lightOnSound);
        potal.gameObject.SetActive(true);
    }

   
}
