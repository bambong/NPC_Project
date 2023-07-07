using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class StateController<T>
    {
        private T owner;
        
        protected IState<T> curState;
        protected IState<T> prevState;
        public IState<T> CurState { get { return curState; } }
        public IState<T> PrevState { get { return prevState; } }
        public StateController(T owner) 
        {
            this.owner = owner;
        }

        public void ChangeState(IState<T> state)
        {
                if(state == curState) 
                {
                    return;
                }
#if DEBUG
                Debug.Log($"상태 전환 {curState} => {state}");
#endif
                curState.Exit(owner);
                prevState = curState;
                curState = state;
                curState.Enter(owner);
        }
        public void RevertToPrevState() 
        {
            ChangeState(prevState);
        }
        public void Update() => curState.UpdateActive(owner);
        public void FixedUpdate() => curState.FixedUpdateActive(owner);

     
    }
