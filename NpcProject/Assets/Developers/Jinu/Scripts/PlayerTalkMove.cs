using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTalkMove : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    private UnityEvent endEvent;
    [SerializeField]
    private PlayerAnimationController.MoveDir exitDir;
    [SerializeField]
    private int talkId;
    private PlayerController playerController;

    void Start()
    {
        playerController = Managers.Game.Player;
    }

    public void MoveToPlayer()
    {
        Managers.Game.SetStateEvent();

        //Vector3 gotovec = playerController.transform.position - startPosition.position;
        //if (gotovec.x > 0)
        //{
        //    playerController.AnimMoveEnter(PlayerAnimationController.MoveDir.Right, gotovec);
        //}
        //else
        //{
        //    playerController.AnimMoveEnter(PlayerAnimationController.MoveDir.Left, gotovec);

        //}
        //playerController.AnimMoveEnter(playerController.transform.position - startPosition.position);
        StartCoroutine(MovePoint());
    }

    IEnumerator MovePoint()
    {
        float distance = 10f;
        
        yield return null;
        Vector3 gotovec = playerController.transform.position - startPosition.position;
        if (gotovec.x > 0)
        {
            playerController.AnimMoveEnter(PlayerAnimationController.MoveDir.Right, gotovec);
        }
        else
        {
            playerController.AnimMoveEnter(PlayerAnimationController.MoveDir.Left, gotovec);

        }

        while (distance > 0.5f)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, startPosition.position, 5f * Time.deltaTime);
            distance = Vector3.Distance(playerController.transform.position, startPosition.position);
            yield return null;
        }
        //playerController.transform.position = startPosition.position;
        playerController.AnimIdleEnter(exitDir);
        var talk = Managers.Talk.GetTalkEvent(talkId);
        talk.OnComplete(() => endEvent?.Invoke());
        //Talk Event Start
        Managers.Talk.PlayCurrentSceneTalk(talkId);
    }
}
