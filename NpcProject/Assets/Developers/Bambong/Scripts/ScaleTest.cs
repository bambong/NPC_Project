using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ScaleTest : MonoBehaviour
{
    [SerializeField]
    private BoxCollider col;
    [SerializeField]
    private float speed =2; 
    private GameObject parentTemp;
    private Vector3 targetScale;
    private Vector3 originScale;
    [SerializeField]
    private bool isOn = false;
    private void Awake()
    {

        originScale = transform.lossyScale;    
        targetScale = new Vector3(11,11,11);
        parentTemp = new GameObject();
       // parentTemp.hideFlags = HideFlags.HideInHierarchy;
        //var pos = transform.position;
        //pos.y -= 0.5f;
        //parentTemp.transform.position = pos;
        //transform.SetParent(parentTemp.transform);
        //parentTemp.transform.localScale = targetScale;
        //transform.SetParent(null);

    }
    private Vector3 VectorMultipleScale(Vector3 origin,Vector3 scale)
    {
        origin.x *= scale.x;
        origin.y *= scale.y;
        origin.z *= scale.z;
        return origin;
    }
    public static float InSine(float t) => (float)Math.Cos(t * Math.PI / 2);
    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.X)) 
        {
            transform.DOScale(originScale,1f);
        }
        if(!isOn) 
        {
            return;
        }

        if(transform.lossyScale.magnitude < targetScale.magnitude) 
        {
            //speed = InSine(transform.lossyScale.magnitude / targetScale.magnitude)*10;
            var curFrameDesirScale = transform.lossyScale + (originScale * Time.deltaTime * speed) ; 
            if(curFrameDesirScale.magnitude > targetScale.magnitude) 
            {
                curFrameDesirScale = targetScale;
            }
            var desireBoxSize = VectorMultipleScale(col.size / 2, curFrameDesirScale);
            var curBoxSize = VectorMultipleScale(col.size / 2, transform.lossyScale);
            var boxScaleDiff = (desireBoxSize - curBoxSize); 

  
            Vector3 parentPos = Vector3.zero;
            Vector3 rayBox = curBoxSize * 0.99f;
            if (CheckAxis(transform.right * desireBoxSize.x, new Vector3(0, rayBox.y, rayBox.z), boxScaleDiff.x, curBoxSize.x, ref parentPos))
            {
                return;
            }
            if (CheckAxis(transform.up * desireBoxSize.y, new Vector3(rayBox.x, 0, rayBox.z), boxScaleDiff.y, curBoxSize.y, ref parentPos)) 
            {
                return;
            }
            if (CheckAxis(transform.forward * desireBoxSize.z, new Vector3(rayBox.x, rayBox.y, 0), boxScaleDiff.z, curBoxSize.z, ref parentPos))
            {
                return;
            }
            //{
            //    Vector3 rayDis = transform.up * desireBoxSize.y;
            //    Vector3 rayBox = curBoxSize*0.99f;
            //    rayBox.y = 0;
            //    if(RayTest(rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(-rayDis,boxScaleDiff.y),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos += new Vector3(0,curBoxSize.y,0);
            //    }
            //    else if(RayTest(-rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(rayDis,boxScaleDiff.y),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos -= new Vector3(0,curBoxSize.y,0);
            //    }
            //}
            //{
            //    Vector3 rayDis = transform.right * desireBoxSize.x;
            //    Vector3 rayBox = curBoxSize * 0.99f;
            //    rayBox.x = 0;
            //    if(RayTest(rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(-rayDis,boxScaleDiff.x),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos += new Vector3(curBoxSize.x,0,0);
            //    }
            //    else if(RayTest(-rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(rayDis,boxScaleDiff.x),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos -= new Vector3(curBoxSize.x,0,0);
            //    } 
            //}
            //{
            //    Vector3 rayDis = transform.forward * desireBoxSize.z;
            //    Vector3 rayBox = curBoxSize * 0.99f;
            //    rayBox.z = 0;
            //    if(RayTest(rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(-rayDis,boxScaleDiff.z),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos += new Vector3(0,0,curBoxSize.z);
            //    }
            //    else if(RayTest(-rayDis,rayBox))
            //    {
            //        if(RayTest(AddDis(rayDis,boxScaleDiff.z),rayBox))
            //        {
            //            return;
            //        }
            //        parentPos -= new Vector3(0,0,curBoxSize.z);
            //    }
            //}

            //pos += parentPos.x * transform.right;
            //pos += parentPos.y * transform.up;
            //pos += parentPos.z * transform.forward;
   
            parentTemp.transform.localScale = transform.lossyScale;
            parentTemp.transform.rotation = transform.rotation;
            parentTemp.transform.position = transform.position + parentPos;
            transform.SetParent(parentTemp.transform);
            parentTemp.transform.localScale = curFrameDesirScale;
            transform.SetParent(null);
        }
    }
    private bool CheckAxis(Vector3 rayDir ,Vector3 rayBox,float boxScaleDiff ,float parentPosFactor, ref Vector3 parentPos)
    {
        if (RayTest(rayDir, rayBox))
        {
            if (RayTest(AddDis(-rayDir, boxScaleDiff), rayBox))
            {
                return true;
            }
            parentPos += rayDir.normalized * parentPosFactor;
        }
        else if (RayTest(-rayDir, rayBox))
        {
            if (RayTest(AddDis(rayDir, boxScaleDiff), rayBox))
            {
                return true;
            }
            parentPos -= rayDir.normalized * parentPosFactor;
        }
        return false;
    }
    private Vector3 AddDis(Vector3 vec , float amount) 
    {
        return vec + vec.normalized * amount;
    }
    private bool RayTest(Vector3 vec, Vector3 boxSize) 
    {
        var pos = col.transform.position;

        RaycastHit hit;
        int layer = 1;
        foreach(var name in Enum.GetNames(typeof(Define.ColiiderMask)))
        {
            layer += (1 << (LayerMask.NameToLayer(name)));
        }
        //var boxSize = VectorMultipleScale(col.size / 2,transform.lossyScale);
#if UNITY_EDITOR
        ExtDebug.DrawBoxCastBox(pos,boxSize,transform.rotation,vec.normalized,vec.magnitude,Color.blue);
#endif
        Physics.BoxCast(pos,boxSize,vec.normalized,out hit,transform.rotation,vec.magnitude,layer);
        //Physics.BoxCastAll(pos,boxSize,vec.normalized,transform.rotation,vec.magnitude,layer);
  
        if(hit.collider != null )
        {
            Debug.Log($"hit : {hit.collider.name}");
            return true;
        }

        return false;
    }
}
