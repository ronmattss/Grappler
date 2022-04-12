using System;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class PlatformController : MonoBehaviour
    {
        public int id;

        private void OnEnable()
        {
            Subscribe();
        }
        
        private void OnDisable()
        {
            UnSubscribe();
        }


        public void OnButtonPush(int id)
        {
            if (id == this.id)
            {
                LeanTween.moveY(gameObject, 3, 1f);
            }
        }
       public void OnButtonRelease(int id)
        {
            if (id == this.id)
            {
                LeanTween.moveY(gameObject, 0, 1f);
            }
        }
       
       void Subscribe()
       {
           LevelEvents.Instance.onTriggerEnter.AddListener(OnButtonPush);
           LevelEvents.Instance.onTriggerExit.AddListener(OnButtonRelease);
       }
       void UnSubscribe()
        {
            LevelEvents.Instance.onTriggerEnter.RemoveListener(OnButtonPush);
            LevelEvents.Instance.onTriggerExit.RemoveListener(OnButtonRelease);
        }
       




    }
}