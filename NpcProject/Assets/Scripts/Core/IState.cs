using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IState<T>
    {
        public void Enter(T stateController);
        public void UpdateActive(T stateController);
        public void FixedUpdateActive(T stateController);
        public void Exit(T stateController);
    }
  
