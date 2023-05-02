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

public class MiniGameManager : MonoBehaviour
{
    public enum SELECT_TYPE 
    {
        Row = 1 ,
        Column = -1
    }

    [SerializeField]
    private string orderString = "A1,D2,B3,F4";
   
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private Transform orderParent;
    [SerializeField]
    private Transform resultParent;

    [SerializeField]
    private List<ResultNodeController> orderNodes = new List<ResultNodeController>();
    [SerializeField]
    private List<ResultNodeController> resultNodes = new List<ResultNodeController>();

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

    private int curResultIndex = 0;
    private string[] curGameKeys;
    private List<List<MiniGameNodeController>> nodeMap = new List<List<MiniGameNodeController>>();
    private SELECT_TYPE curSelectType = SELECT_TYPE.Row;
    private Vector2Int pointIndex = Vector2Int.zero;

    private bool iSGameStart = false;
    private bool isGameEnd = false;
    private float curTime;
    private bool isTimeLimit = false;
    private void Start()
    {
        for (int i = 0; i < row; i++)
        {
            nodeMap.Add(new List<MiniGameNodeController>());
        }
        DOTween.SetTweensCapacity(200, 100);
        InitOrderNode();
        InitResultNode();
        InitLayOut();
        InitNode();
        InitPuzzle();
        SetStateInit();
        timePanelGroup.alpha = 0;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(2f);
        s.Append(backGround.DOFade(0, 2f));
        s.Play();
        orderPanel.Open(2f, () => { OpenOrderNode(); OpenResultNode(); });
        gamePanel.Open(1.8f, () =>
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => { OpenGameNodes(); timePanelGroup.DOFade(1, 1f); });
            seq.Play();
        });
    }

    public void InitOrderNode()
    {
        var splitStrs = orderString.Split(',');
        curGameKeys = splitStrs.Distinct().ToArray();
        for (int i =0; i < splitStrs.Length; i++) 
        {
            var temp = CreateResultNode(orderParent);
            orderNodes.Add(temp);
            temp.ClearSize();
            temp.SetKey(splitStrs[i]);
        }  
    }
    public void InitResultNode()
    {
        for (int i = 0; i < orderNodes.Count; i++)
        {
            var temp = CreateResultNode(resultParent);
            resultNodes.Add(temp);
            temp.ClearSize();
            temp.SetTextUI("");
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
    public void ResetPuzzle() 
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                nodeMap[i][j].SetKey(curGameKeys[Random.Range(0, curGameKeys.Length)]);
            }
        }
        InitPuzzle();
    }
    public void InitPuzzle() 
    {
        //r * c 짝수인지 홀수인지 확인
        int maxCount = (row * column) % 2 == 0 ? row * column : row * column - 2;
        if (orderNodes.Count > maxCount) 
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
        
        while (puzzleStack.Count < orderNodes.Count)
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
                    continue;
                }
                randomY = pickIndex.RemoveRandom();

            }
            curType = (SELECT_TYPE)(-(int)curType);

            puzzleStack.Push(new Vector2Int(randomX, randomY));
            puzzle[randomX, randomY] = true;
        } 
        for(int i =0; i < orderNodes.Count; ++i) 
        {
            var peek = puzzleStack.Peek();
            puzzleStack.Pop();
            nodeMap[peek.x][peek.y].SetKey(orderNodes[i].AnswerKey);
            nodeMap[peek.x][peek.y].TestAnswerMod();
        }
    }
    public ResultNodeController CreateResultNode(Transform parent)
    {
        var node = Managers.UI.MakeSubItem<ResultNodeController>(parent, "MiniGame/ResultNode");
        return node;
    }
    public void ClickNode(MiniGameNodeController node)
    {

        if (!iSGameStart)
        {
            SetStateGamePlay();
        }
        resultNodes[curResultIndex].SetKey(node.AnswerKey);
        curSelectType = (SELECT_TYPE)(-(int)curSelectType);
        pointIndex = node.PosIndex;
        resultNodes[curResultIndex].SetIsSuccess(resultNodes[curResultIndex].AnswerKey == orderNodes[curResultIndex].AnswerKey);
        if(!resultNodes[curResultIndex].IsSuccess)
        {
            SetStateGameOver();
            return;
        }
        curResultIndex++;

        if(curResultIndex > resultNodes.Count()-1) 
        {
            //for (int i =0; i < resultNodes.Count; ++i) 
            //{
            //    if (!resultNodes[i].IsSuccess) 
            //    {
            //        SetStateGameOver();
            //        return;
            //    }
            //}
            SetStateGameClear();
            return;
        }

        UpdateEnableNode(pointIndex, curSelectType);
    }
    public void PointEnter(Vector2Int point)
    {
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
        for (int i = 0; i < orderNodes.Count; i++)
        {
            orderNodes[i].OpneAnim(interval);
            resultNodes[i].OpneAnim(interval);
            interval += 0.1f;
        }
    }
    public void OpenOrderNode() 
    {
        float interval = 0;
        for (int i = 0; i < orderNodes.Count; i++)
        {
            orderNodes[i].OpneAnim(interval);
            interval += 0.1f;
        }
    }
    public void OpenResultNode() 
    {
        float interval = 0;
        for (int i = 0; i < orderNodes.Count; i++)
        {
            resultNodes[i].OpneAnim(interval);
            interval += 0.1f;
        }
    }
    private void CloseNodes() 
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
        for (int i = 0; i < orderNodes.Count; i++)
        {
            orderNodes[i].CloseAnim(interval);
            resultNodes[i].CloseAnim(interval);
            interval += 0.1f;
        }
        //Sequence seq = DOTween.Sequence();
        //seq.AppendInterval(row * 0.1f + 0.2f);
        //seq.AppendCallback(() => { gamePanel.Close(); orderPanel.Close(); });
        //seq.Play();
    }
    public void SetStateGameOver() 
    {
        if(isGameEnd)
        {
            return;
        }

        isGameEnd = true;
        if (timeLimitEffect.IsPlaying)
        {
            timeLimitEffect.StopFeedbacks();
        }
        SetResultText(false);
        CloseNodes();
    }
    public void SetStateGameClear()
    {
        isGameEnd = true;

        if (timeLimitEffect.IsPlaying)
        {
            timeLimitEffect.StopFeedbacks();
        }
        SetResultText(true);
        CloseNodes();
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
        curTime = 0;
        isGameEnd = false;
        iSGameStart = false;
        timeText.text = timeLimit.ToString("00.00");
        timeFillGauge.fillAmount = 0;
    }
  
    public void SetResultText(bool isSuccess) 
    {
        string targetText = "";
        if (isSuccess)
        {
            targetText = "SUCCESS";
            resultText.color = Color.green;
        }
        else 
        {
            targetText = "FAIL";
            resultText.color = Color.red;
        }
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(orderNodes.Count * 0.1f );
        seq.Append(resultText.DOText(targetText, 0.5f));
        seq.Play();
    }

}
