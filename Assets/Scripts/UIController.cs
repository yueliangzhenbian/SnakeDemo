using System;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private StartView startView;

        [SerializeField]
        private PlayingView playingView;

        [SerializeField]
        private GameOverView gameOverView;

        public event Action OnStartClicked;
        public event Action OnExitClicked;
        public event Action OnBackToStartClicked;

        public IMoveInputSource GetJoystickInput()
        {
            return playingView != null ? playingView.JoystickInput : null;
        }

        private void Awake()
        {
            ApplyUiFont();
            BindViews();
        }

        public void ShowStart()
        {
            ShowView(startView);
        }

        public void ShowPlaying()
        {
            ShowView(playingView);
        }

        public void ShowGameOver()
        {
            ShowView(gameOverView);
        }

        public void SetScore(int score)
        {
            if (playingView != null)
            {
                playingView.SetScore(score);
            }

            if (gameOverView != null)
            {
                gameOverView.SetScore(score);
            }
        }

        private void BindViews()
        {
            if (startView != null)
            {
                startView.OnStartClicked += () => OnStartClicked?.Invoke();
            }

            if (playingView != null)
            {
                playingView.OnExitClicked += () => OnExitClicked?.Invoke();
            }

            if (gameOverView != null)
            {
                gameOverView.OnBackToStartClicked += () => OnBackToStartClicked?.Invoke();
            }
        }

        private void ShowView(UIView target)
        {
            SetViewVisible(startView, target == startView);
            SetViewVisible(playingView, target == playingView);
            SetViewVisible(gameOverView, target == gameOverView);
        }

        private static void SetViewVisible(UIView view, bool visible)
        {
            if (view == null)
            {
                return;
            }

            if (visible)
            {
                view.Show();
            }
            else
            {
                view.Hide();
            }
        }

        private void ApplyUiFont()
        {
            Font uiFont = Font.CreateDynamicFontFromOSFont(
                new[] { "Microsoft YaHei", "PingFang SC", "Noto Sans CJK SC", "Arial Unicode MS", "Arial" },
                32);

            Text[] texts = GetComponentsInChildren<Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].font = uiFont;
            }
        }
    }
}
