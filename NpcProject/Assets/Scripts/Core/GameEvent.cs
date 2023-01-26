using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class GameEvent 
{
    protected Action onComplete;
    protected Action onStart;
    public abstract void Play();
 
    public void OnStart(Action action)
    {
        onStart += action;
    }
    public void OnStart(GameEvent gameEvent)
    {
        onStart += gameEvent.Play;
    }

    public void OnComplete(Action action)
    {
        onComplete += action;
    }
    public void OnComplete(GameEvent gameEvent)
    {
        onComplete += gameEvent.Play;
    }
}
public class GameSequence : GameEvent
{
    private GameEvent firstEvent;
    private GameEvent lastEvent;
    public GameSequence AddEvent(GameEvent gameEvent)
    {
        if(firstEvent == null) 
        {
            firstEvent = gameEvent;
            lastEvent = gameEvent;
        }
        else 
        {
            lastEvent.OnComplete(gameEvent);
            lastEvent = gameEvent;
        }
        return this;
    }
    public override void Play()
    {
        onStart?.Invoke();
        firstEvent.Play();
    }



}