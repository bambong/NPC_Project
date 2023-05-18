using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Poolable : MonoBehaviour
{
	public bool IsUsing;
	private bool isInit;
	public UnityEvent onCreate;
	public UnityEvent onReturn;

	private void Start()
	{
		Init();
    }
	public void Init() 
	{
		if(isInit)
		{
			return;
		}
		isInit = true;
		OnCreate();
	}
	public virtual void OnCreate()
	{
		onCreate?.Invoke();
    }
	public void Return()
	{
		if(!isInit)
		{
			return;
		}
		isInit = false;
		OnReturnToPool();
	}
	public virtual void OnReturnToPool() 
	{
        onReturn?.Invoke();
    }
}
