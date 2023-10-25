using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharController : MonoBehaviour, IInteraction
{
    [SerializeField]
    private int talk;

    [SerializeField]
    private int repeatTalk;

    [SerializeField]
    private CharacterInfo left;
    [SerializeField]
    private CharacterInfo right;

    public GameObject Go => gameObject;

    public bool IsInteractAble => true;

    private bool isPlay = false;


    public void OnInteraction()
    {
        if (isPlay) 
        {
            var retalkevent = Managers.Talk.GetTalkEvent(repeatTalk);
            
            if(left != null)
            {
                retalkevent.OnStart(() => retalkevent.left = left);
            }
            if(right != null)
            {
                retalkevent.OnStart(() => retalkevent.right = right);
            }

            retalkevent.Play();
            return;
        }
        isPlay = true;

        var talkevent = Managers.Talk.GetTalkEvent(talk);
        if (left != null)
        {
            talkevent.OnStart(() => talkevent.left = left);
        }
        if (right != null)
        {
            talkevent.OnStart(() => talkevent.right = right);
        }
        talkevent.Play();         
    }
}
