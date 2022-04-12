using System;
using DefaultNamespace.Manager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class ExitInteractable : Interactable
    {
        public LevelData levelData;
        private GameObject currentLevel;
        public UnityEvent OnExit;

        public void OnEnable()
        {
            OnExit.AddListener(UIManager.Instance.ChangeScoreText);
        }



        public  override void Interact()
        {
            // load data from levelData
            GameManager.Instance.LoadLevel(levelData);
            currentLevel = gameObject.transform.parent.gameObject;
            OnExit.Invoke();
            currentLevel.SetActive(false);
            base.Interact();

        }

    }
    
}