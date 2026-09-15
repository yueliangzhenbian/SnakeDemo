using System;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class PlayingView : UIView
    {
        [SerializeField]
        private Button exitButton;

        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private JoystickView joystickView;

        public event Action OnExitClicked;

        public IMoveInputSource JoystickInput => joystickView;

        private void Awake()
        {
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());
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
