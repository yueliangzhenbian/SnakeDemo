using System;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class GameOverView : UIView
    {
        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Button backToStartButton;

        public event Action OnBackToStartClicked;

        private void Awake()
        {
            if (backToStartButton != null)
            {
                backToStartButton.onClick.AddListener(() => OnBackToStartClicked?.Invoke());
            }
        }

        public void SetScore(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"分数: {score}";
            }
        }
    }
}
