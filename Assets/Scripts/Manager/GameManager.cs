using System;
using Cinemachine;
using Controls;
using GrapplingHook;
using Manager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        // will handle the game state
        private struct LevelDetails
        {
            public GameObject playerPosition;

            public  GameObject levelPrefab;
            public GameObject currentLevel;
            
        }


        public  GameObject player;

        [SerializeField] private LevelData initialLevel;
        [SerializeField]private LevelDetails newLevel;
        [SerializeField] private GameState currentGameState;
        public UnityEvent OnEscapePress;
        public void LoadLevel(LevelData levelData)
        {
            ResetPlayer();
            newLevel.levelPrefab = levelData.levelPrefab;
            newLevel.playerPosition = newLevel.levelPrefab.transform.Find("PlayerStart").gameObject;
            ChangeCamera(newLevel.levelPrefab.transform.Find("PrimaryCamera").gameObject.GetComponent<CinemachineVirtualCamera>(), levelData.followCamera);

            StartGame();
        }

        public void ExitLevel()
        {
            Destroy(newLevel.currentLevel);
            UIManager.Instance.ShowMainMenuUI();
        }

        private void Awake()
        {
          //  LoadLevel(initialLevel);
          
        }

        private void LateUpdate()
        {
            OnEscape();
        }

        public void OnEscape()
        {
            if (PlayerBaseControls.Instance.GetEscapeInput() && currentGameState == GameState.InGame)
            {
                OnEscapePress.Invoke();
                UIManager.Instance.ShowMainMenuUI();
            }
            
                
        }

        void ChangeCamera(CinemachineVirtualCamera newCamera, bool follow)

        {
            CameraTransitionManager.Instance.currentVirtualCamera = newCamera;
            if (follow)
            {
                CameraTransitionManager.Instance.currentVirtualCamera.Follow = player.transform;
            }
            
        }
        

        public void StartGame()
        {
            // load the level
            newLevel.currentLevel =  Instantiate(newLevel.levelPrefab);
            newLevel.currentLevel.SetActive(true);

            player.transform.position = newLevel.playerPosition.transform.position;
            
            // set the player position
            
        }

        public void SetPlayerPosition()
        {
            player.transform.position = newLevel.playerPosition.transform.position;

        }

        public void ChangeState(int state)
        {
            currentGameState = (GameState)state;
        }

        void ResetPlayer()
        {
            player.transform.parent = null;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<GrapplingHookGun>().hookPoint.transform.parent = null;
          //  player.GetComponent<GrapplingHookGun>().enabled = false;


        }
    }
}