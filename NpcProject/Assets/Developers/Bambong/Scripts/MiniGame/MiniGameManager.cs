using DG.Tweening;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

using Random = UnityEngine.Random;

[Serializable]
public class MiniGameColorGroupData 
{
    public int Key;
    public string OrderKey;
}

public class MiniGameManager : MonoBehaviour
{
    public enum SELECT_TYPE 
    {
        Row = 1 ,
        Column = -1
    }

    [SerializeField]
    private List<MiniGameColorGroupData> miniGameColorGroupDatas;
    [SerializeField]
    private List<Color> orderKeyColors;

    [SerializeField]
    private int[] answerColorKey;

    [SerializeField]
    private string[] answerKey;

    [SerializeField]
    private string[] curGameKeys;
   
   
    [SerializeField]
    private ResultPanelController resultText;
    [SerializeField]
    private OrderColorLayoutController orderColorLayout;
    [SerializeField]
    private Transform orderParent;
    [SerializeField]
    private Transform resultParent;
    [SerializeField]
    private CanvasGroup timePanelGroup;
    [SerializeField]
    private CanvasGroup backGround;
    [Header("Time Gauge")]
    [SerializeField]
    private Image timeFillGauge;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private float timeLimit;

    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    private int row = 1; 
    [SerializeField]
    private int column =1;
    [SerializeField]
    private float space = 10f;
    [SerializeField]
    private float childSize =1;

    [Header("MMF Effect")]
    [SerializeField]
    private MMF_Player timeLimitEffect;

    [SerializeField]
    private MiniGamePanelController gamePanel;

    [SerializeField]
    private MiniGamePanelController orderPanel;
    [SerializeField]
    private MiniGamePanelController rulePanel;

    [SerializeField]
    private CanvasGroup ruleTextGroup;

    private List<ColorOrderGroupController> orderColorGroups = new List<ColorOrderGroupController>();
    private List<ResultColorNodeController> resultColorNodes = new List<ResultColorNodeController>();

    private List<List<MiniGameNodeController>> nodeMap = new List<List<MiniGameNodeController>>();
    
    private Vector2Int pointIndex;

    private SELECT_TYPE curSelectType = SELECT_TYPE.Row;
    private int curResultIndex = 0;
    private bool iSGameStart = false;
    private bool isGameEnd = false;
    private bool isTimeLimit = false;
    private float curTime;
    private void Start()
    {
        for (int i = 0; i < row; i++)
        {
            nodeMap.Add(new List<MiniGameNodeController>());
        }
        DOTween.SetTweensCapacity(200, 100);
        orderColorLayout.Init(answerColorKey, this);
        InitOrderNode();
        InitResultNode();
        InitLayOut();
        InitNode();
        InitPuzzle();
        SetStateInit();
        timePanelGroup.alpha = 0;
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(2f);
        //s.Append(backGround.DOFade(0, 2f));
        //s.Play();
        OpenPanel(1.8f);
    }
    public void OpenPanel(float interval) 
    {
        ruleTextGroup.alpha = 0;
        orderPanel.Open(interval+0.2f,() => { OpenOrderNode(); OpenResultNode(); });
        rulePanel.Open(interval + 0.4f, () => { ruleTextGroup.DOFade(1, 0.5f); });
        gamePanel.Open(interval,() =>
        {
            backGround.DOFade(0, 2f);
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => { OpenGameNodes(); timePanelGroup.DOFade(1,1f); });
            seq.Play();
        });
    }
    public void ClosePanel(Action action) 
    {
        CloseNodes();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval((2*(row-1)+1) * 0.1f + 0.2f);
        seq.AppendCallback(() => { gamePanel.Close(); ruleTextGroup.DOFade(0,0.2f).OnComplete(()=> { rulePanel.Close(); }); orderPanel.Close(action); });
        seq.Play();
    }

    public void InitOrderNode()
    {
        for (int i =0; i < miniGameColorGroupDatas.Count; i++) 
        {
            var temp = CreateColorOrder(orderParent);
            temp.InitData(this, miniGameColorGroupDatas[i]);
            orderColorGroups.Add(temp);
           // temp.SetKey(splitStrs[i]);
        }  
    }
    public void InitResultNode()
    {
        for (int i = 0; i < answerColorKey.Length; i++)
        {
            var temp = CreateResultColorNode(resultParent);
            resultColorNodes.Add(temp);
            temp.ClearSize();
            //temp.SetTextUI("");
        }
    }
    public void InitLayOut() 
    {
        gridLayoutGroup.cellSize = new Vector2(childSize,childSize);
        gridLayoutGroup.spacing = new Vector2(space,space);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayoutGroup.constraintCount = row;
    }
    public void InitNode() 
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                nodeMap[i].Add(CreateNode(new Vector2Int(i, j)));
            }
        }
    }
    public MiniGameNodeController CreateNode(Vector2Int pos) 
    {
         var node = Managers.UI.MakeSubItem<MiniGameNodeController>(gridLayoutGroup.transform , "MiniGame/MiniGameNode") ;
        node.SetData(this, pos, curGameKeys[Random.Range(0, curGameKeys.Length)]);
        return node;
    }
    public ColorOrderGroupController CreateColorOrder(Transform parent)
    {
        var group = Managers.UI.MakeSubItem<ColorOrderGroupController>(parent, "MiniGame/ColorOrderGroup");
        return group;
    }
    public void ResetPuzzle() 
    {

        StopAllCoroutines();
        SetStateInit();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                nodeMap[i][j].ResetNode();
                nodeMap[i][j].SetKey(curGameKeys[Random.Range(0, curGameKeys.Length)]);
            }
        }
        for (int i = 0; i < orderColorGroups.Count; i++)
        {
            orderColorGroups[i].ResetOrderGroup();
        }
      
        OpenPanel(0);
        for(int i = 0; i < resultColorNodes.Count; i++)
        {
            resultColorNodes[i].ResetNode();
        }
        InitPuzzle();
    }
    public void InitPuzzle() 
    {
        //r * c 짝수인지 홀수인지 확인
        int maxCount = (row * column) % 2 == 0 ? row * column : row * column - 2;
        if (answerKey.Length > maxCount) 
        {
            Debug.LogError("현재 노드 갯수로 불가능한 퍼즐 길이입니다.");
            return;
        }
        //int[][] a = new int[row][column];
        bool[,] puzzle = new bool[row, column];
        //for(int i =0; i < row; ++i) 
        //{
        //    puzzle.Add(new List<int>());
        //    for (int j = 0; j < column; ++j)
        //    {
        //        puzzle[j].Add(0);
        //    }
        //}

        SELECT_TYPE curType = curSelectType;
        int randomX = Random.Range(0, row);
        int randomY = Random.Range(0, column);

        Stack<Vector2Int> puzzleStack = new Stack<Vector2Int>();
        puzzleStack.Push(new Vector2Int(randomX, randomY));
        puzzle[randomX, randomY] = true;
        List<Vector2Int> clearList = new List<Vector2Int>();
        
        while (puzzleStack.Count < answerKey.Length)
        {   
            if(curType == SELECT_TYPE.Row)
            {
                List<int> pickIndex = new List<int>();
                for(int i=0; i < row; ++i) 
                {
                    if (puzzle[i, randomY] == false) 
                    {
                        pickIndex.Add(i);    
                    }
                }
                if(pickIndex.Count <= 0) 
                {
                    if(puzzleStack.Count <= 0)
                    {
                        Debug.LogError("퍼즐 구성 실패!!");
                        return;
                    }
                    for(int i =0; i < clearList.Count; ++i) 
                    {
                        puzzle[clearList[i].x, clearList[i].y] = false;
                    }
                    clearList.Clear();
                    clearList.Add(puzzleStack.Peek());
                    puzzleStack.Pop();
                    curType = (SELECT_TYPE)(-(int)curType);
                    continue;
                }
                randomX = pickIndex.RemoveRandom();
            }
            else 
            {
                List<int> pickIndex = new List<int>();
                for (int i = 0; i < column; ++i)
                {
                    if (puzzle[randomX, i] == false)
                    {
                        pickIndex.Add(i);
                    }
                }
                if (pickIndex.Count <= 0)
                {
                    if (puzzleStack.Count <= 0)
                    {
                        Debug.LogError("퍼즐 구성 실패!!");
                        return;
                    }
                    for (int i = 0; i < clearList.Count; ++i)
                    {
                        puzzle[clearList[i].x, clearList[i].y] = false;
                    }
                    clearList.Clear();
                    clearList.Add(puzzleStack.Peek());
                    puzzleStack.Pop();
                    curType = (SELECT_TYPE)(-(int)curType);
                    continue;
                }
                randomY = pickIndex.RemoveRandom();

            }
            curType = (SELECT_TYPE)(-(int)curType);

            puzzleStack.Push(new Vector2Int(randomX, randomY));
            puzzle[randomX, randomY] = true;
        } 
        for(int i = answerKey.Length-1; i >= 0; --i) 
        {
            var peek = puzzleStack.Peek();
            puzzleStack.Pop();
            nodeMap[peek.x][peek.y].SetKey(answerKey[i]);
            nodeMap[peek.x][peek.y].TestAnswerMod();
        }
    }
    public ResultColorNodeController CreateResultColorNode(Transform parent)
    {
        var node = Managers.UI.MakeSubItem<ResultColorNodeController>(parent, "MiniGame/ResultColorNode");
        return node;
    }
    public void PushColorNode(int key)
    {
        if (answerColorKey[curResultIndex] != key)
        {
            SetStateGameReset();
            return;
        }
        resultColorNodes[curResultIndex].SetInnerColor(orderKeyColors[key]);
        resultColorNodes[curResultIndex].SetIsSuccess(true);
        curResultIndex++;

        if(curResultIndex >= answerColorKey.Length) 
        {
            SetStateGameClear();
        }
    }
    public void ClickNode(MiniGameNodeController node)
    {

        if (!iSGameStart)
        {
            SetStateGamePlay();
        }

        curSelectType = (SELECT_TYPE)(-(int)curSelectType);
        pointIndex = node.PosIndex;

        for(int i =0; i < orderColorGroups.Count; ++i)
        {
            orderColorGroups[i].PushKey(node.AnswerKey);
            if (isGameEnd)
            {
                return;
            }
        }

        UpdateEnableNode(pointIndex, curSelectType);
    }
    public void PointEnter(Vector2Int point)
    {
        for(int i = 0; i < orderColorGroups.Count; ++i) 
        {
            orderColorGroups[i].PointerEnter(nodeMap[point.x][point.y].AnswerKey);
        }

        SELECT_TYPE type = (SELECT_TYPE)(-(int)curSelectType);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector2Int temp = new Vector2Int(i, j);
                if(point == temp) 
                {
                    continue;
                }
                switch (type)
                {
                    case SELECT_TYPE.Row:
                        temp.y = point.y;
                        break;
                    case SELECT_TYPE.Column:
                        temp.x = point.x;
                        break;
                }
                if (point == temp)
                {
                    nodeMap[i][j].SetLookUpmod();
                }
            }
        }
    }
    public void PointExit(Vector2Int point)
    {
        for (int i = 0; i < orderColorGroups.Count; ++i)
        {
            orderColorGroups[i].PointerExit(nodeMap[point.x][point.y].AnswerKey);
        }
        SELECT_TYPE type = (SELECT_TYPE)(-(int)curSelectType);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector2Int temp = new Vector2Int(i, j);
                if (point == temp)
                {
                    continue;
                }
                switch (type)
                {
                    case SELECT_TYPE.Row:
                        temp.y = point.y;
                        break;
                    case SELECT_TYPE.Column:
                        temp.x = point.x;
                        break;
                }
                if (point == temp)
                {
                    nodeMap[i][j].ClearLookUpmod();
                }
            }
        }
    }
    public void UpdateEnableNode(Vector2Int point , SELECT_TYPE type)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector2Int temp = new Vector2Int(i, j);
                switch (type)
                {
                    case SELECT_TYPE.Row:
                        temp.y = pointIndex.y;
                        break;
                    case SELECT_TYPE.Column:
                        temp.x = pointIndex.x;
                        break;
                }
                if (point == temp)
                {
                    nodeMap[i][j].EnableNode();
                }
                else
                {
                    nodeMap[i][j].DisableNode();
                }
            }
        }
    }
    public IEnumerator TimeUpdate() 
    {
        while (!isGameEnd) 
        {
            curTime += Time.deltaTime;
         
            if(curTime > timeLimit) 
            {
                timeText.text = "00.00";
                SetStateGameOver();
                yield break;
            }
            if (!isTimeLimit) 
            {
                if (curTime / timeLimit > 0.7f) 
                {
                    isTimeLimit = true;
                    timeLimitEffect.PlayFeedbacks();
                }
            }
           
            TimeTextUpdate();
            TimeBarUpdate();
            yield return null;
        }
    
    }
    private void TimeTextUpdate() 
    {
        timeText.text = (timeLimit - curTime).ToString("00.00");
    }
    private void TimeBarUpdate() 
    {
        timeFillGauge.fillAmount = curTime / timeLimit;
    }
    private void OpenGameNodes() 
    {
        float interval = 0;
        for (int i = 0; i < row - 1; i++)
        {
            var temp = i;
            for (int j = 0; temp >= 0; j++)
            {
                nodeMap[temp][j].OpenAnim(interval);
                temp--;
            }
            interval += 0.1f;
        }
        for (int i = 0; i < row; i++)
        {
            var temp = i;
            for (int j = row-1 ; temp < row; j--)
            {
                nodeMap[temp][j].OpenAnim(interval);
                temp++;
            }
            interval += 0.1f;
        }
        interval = 0;
        for (int i = 0; i < orderColorGroups.Count; i++)
        {
            orderColorGroups[i].OpenAnim(interval);
            interval += 0.1f;
        }
        interval = 0;
        for (int i = 0; i < answerColorKey.Length; i++)
        {
            resultColorNodes[i].OpenAnim(interval);
            interval += 0.1f;
        }
       

    }
    public void OpenOrderNode() 
    {
        float interval = 0;
        for (int i = 0; i < orderColorGroups.Count; i++)
        {
            orderColorGroups[i].OpenAnim(interval);
            interval += 0.1f;
        }
    }
    public void OpenResultNode() 
    {
        float interval = 0;
        for (int i = 0; i < resultColorNodes.Count; i++)
        {
            resultColorNodes[i].OpenAnim(interval);
            interval += 0.1f;
        }
    }
    private void CloseNodes(Action action = null) 
    {
        float interval = 0;
        for (int i = 0; i < row - 1; i++)
        {
            var temp = i;
            for (int j = 0; temp >= 0; j++)
            {
                nodeMap[temp][j].CloseAnim(interval);
                temp--;
            }
            interval += 0.1f;
        }
        for (int i = 0; i < row; i++)
        {
            var temp = i;
            for (int j = row - 1; temp < row; j--)
            {
                nodeMap[temp][j].CloseAnim(interval);
                temp++;
            }
            interval += 0.1f;
        }
        interval = 0;
        for (int i = 0; i < orderColorGroups.Count; i++)
        {
            orderColorGroups[i].CloseAnim(interval);
            interval += 0.1f;
        }
        interval = 0;
        for (int i = 0; i < resultColorNodes.Count; i++)
        {
            resultColorNodes[i].CloseAnim(interval);
            interval += 0.1f;
        }


    }
    public Color GetOrderKeyColor(int key) => orderKeyColors[key];

    public void SetStateGameReset()
    {
        if (isGameEnd)
        {
            return;
        }

        isGameEnd = true;
        iSGameStart = false;
        isTimeLimit = false;
        curSelectType = SELECT_TYPE.Row;
        curResultIndex = 0;

        if (timeLimitEffect.IsPlaying)
        {
            timeLimitEffect.StopFeedbacks();
        }
        
        for (int i = 0; i < orderColorGroups.Count; i++)
        {
            orderColorGroups[i].ResetOrderGroup();
        }
        for (int i = 0; i < resultColorNodes.Count; i++)
        {
            resultColorNodes[i].ResetNode();
        }
        CloseNodes();
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(((2 * (row - 1) + 1) * 0.1f + 0.2f)/2);
        seq.AppendCallback(() => {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    nodeMap[i][j].ResetNode();
                }
            }
            OpenGameNodes();
            isGameEnd = false;
        });
        seq.Play();
    }
    public void SetStateGameOver() 
    {
        if(isGameEnd)
        {
            return;
        }

        isGameEnd = true;
        iSGameStart = false;
        isTimeLimit = false;
        if (timeLimitEffect.IsPlaying)
        {
            timeLimitEffect.StopFeedbacks();
        }
      
        CloseNodes();
        ClosePanel(() => { SetResultText(false); });
    }
    public void SetStateGameClear()
    {
        if (isGameEnd)
        {
            return;
        }
        isGameEnd = true;

        if (timeLimitEffect.IsPlaying)
        {
            timeLimitEffect.StopFeedbacks();
        }
        //SetResultText(true);
        CloseNodes();
        ClosePanel(() => { SetResultText(true); });
    }
    public void SetStateGamePlay() 
    {
        iSGameStart = true;
        StartCoroutine(TimeUpdate());
    }

    public void SetStateInit() 
    {
        if (timeLimitEffect.IsPlaying) 
        {
            timeLimitEffect.StopFeedbacks();
        }
        curSelectType = SELECT_TYPE.Row;
        curResultIndex = 0;
        curTime = 0;
        isGameEnd = false;
        iSGameStart = false;
        isTimeLimit = false;
        timeText.text = timeLimit.ToString("00.00");
        timeFillGauge.fillAmount = 0;
        resultText.ClearText();
    }
  
    public void SetResultText(bool isSuccess) 
    {
 
        Sequence seq = DOTween.Sequence();
        seq.Append(backGround.DOFade(1, 1f));
        seq.Join(timePanelGroup.DOFade(0, 1f));
        seq.Play();

        if (isSuccess)
        {
            resultText.OnSuccess(1f);
        }
        else 
        {
            resultText.OnFail(1f);
        }
       
        //seq.AppendInterval(1);
        //seq.Append(resultText.DOText("PRESS ENTER", 0.5f));
       
    }

}
