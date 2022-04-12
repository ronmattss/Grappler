using TMPro;
using UnityEngine;

namespace DefaultNamespace.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private GameObject _mainMenuGroup;
        [SerializeField] private GameObject _inGameGroup;
        
        
        
        public void ChangeScoreText(int score)
        {
            scoreText.text = $"Score: {score.ToString()}";
        }

        public void ChangeScoreText(string message)
        {
            scoreText.text = message;
        }
        public void ChangeScoreText()
        {
            scoreText.text = "Score: ";
        }
        
        public void ShowMainMenuUI()
        {
            _mainMenuGroup.SetActive(true);
            _inGameGroup.SetActive(false);
        }
        public void ShowInGameUI()
        {
            _mainMenuGroup.SetActive(false);
            _inGameGroup.SetActive(true);
        }
    }
}