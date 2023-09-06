using UnityEngine;

namespace RPG.Scripts.Interfaces
{
    public interface IState
    {
        //Not Used due to the lack of time
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
