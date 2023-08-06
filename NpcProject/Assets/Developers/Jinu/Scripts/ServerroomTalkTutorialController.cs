using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerroomTalkTutorialController : ServerroomTutorialController
{
    [SerializeField]
    private int talkId;

    protected override void Start()
    {
        for (int i = tutorialData.Count; i < pageMarks.Count; ++i)
        {
            pageMarks[i].gameObject.SetActive(false);
        }
        clickNotice.anchoredPosition = startPos;
        rootGroup.alpha = 0;
        innerGroup.alpha = 0;
        renderImage.color = new Color(1, 1, 1, 0);
        descriptionImage.color = new Color(1, 1, 1, 0);

        if (isEventOnce && Managers.Data.IsClearEvent(guId))
        {
            return;
        }
        var talk = Managers.Talk.GetTalkEvent(talkId);
        Managers.Talk.PlayTalk(talk);
        talk.OnComplete(() => Open());
        talk.OnComplete(() => Managers.Data.ClearEvent(guId));
    }
}
