using UnityEngine;
using System;
using UnityEngine.Events;
namespace DefaultNamespace.Manager
{
    // Observer Pattern test
    public class GameEvents : Singleton<GameEvents>
    {

        public Action<int> onButtonEnter;
        public Action<int> onButtonExit;
        public UnityEvent<int> onButtonStay;



        public void ButtonEnter(int id)
        {
            if (onButtonEnter != null)
            {
                onButtonEnter(id);
            }
        }
        public void ButtonExit(int id)
        {
            if (onButtonExit != null)
            {
                onButtonExit(id);
            }
        }
        
        public void ButtonStay(int id)
        {
            
                onButtonStay?.Invoke(id);
            
        }






    }
}