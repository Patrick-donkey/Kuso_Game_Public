using System;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;

namespace RiverCrab
{
    public interface ObjState
    {
        void EnterState();
        void ExcuteState();
        void ExitState();
    }

    public class ObjStateNow : ObjState
    {
        Action enterState;
        Action excuteState;
        Action exitState;

        public ObjStateNow(Action enterstate, Action excutestate, Action exitstate)
        {
            enterState = enterstate;
            excuteState = excutestate;
            exitState = exitstate;
        }

        public void EnterState()
        {
            enterState?.Invoke();
        }

        public void ExcuteState()
        {
            excuteState?.Invoke();
        }

        public void ExitState()
        {
            excuteState?.Invoke();
        }

    }

    public class StateMachine
    {
        ObjState objState;

        public void ChangeState(ObjState objstate)
        {
            if (objState != null)
            {
                objState.ExitState();
            }
            objState = objstate;       
            objState.EnterState();
        }

        public void UpdateState()
        {
            objState.ExcuteState();
        }
    }
}
