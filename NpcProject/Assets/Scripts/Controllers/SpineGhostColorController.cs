using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineGhostColorController : MonoBehaviour
{

    [SerializeField]
    private SkeletonGhost ghost;
    [SerializeField]
    private List<Color> colors;
    
    [SerializeField]
    private float colorTime = 1f;

    private float curTime;
    private int curIndex;
    void Start()
    {

    }

    void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= colorTime) 
        {
            curTime = 0;
            curIndex = (curIndex + 1) % colors.Count;
        }
        int nextIndex = (curIndex+1) % colors.Count;
        ghost.color = Color.Lerp(colors[curIndex], colors[nextIndex],  curTime * (1/ colorTime));
    }
}
