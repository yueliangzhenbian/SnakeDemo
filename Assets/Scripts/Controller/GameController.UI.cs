using UnityEngine;

namespace SnakeGame
{
    public partial class GameController
    {
        [SerializeField]
        private UIController uiController;

        private void BindUI()
        {
            if (uiController == null)
            {
                return;
            }

            uiController.OnStartClicked += StartGame;
            uiController.OnExitClicked += GameOver;
            uiController.OnBackToStartClicked += ReturnToStart;
            BindSnakeMoveInput();
        }

        private void UnbindUI()
        {
            if (uiController == null)
            {
                return;
            }

            uiController.OnStartClicked -= StartGame;
            uiController.OnExitClicked -= GameOver;
            uiController.OnBackToStartClicked -= ReturnToStart;
        }

        private void BindSnakeMoveInput()
        {
            if (snake != null)
            {
                snake.SetMoveExtraSource(uiController.GetJoystickInput());
            }
        }

        private void ShowStartUI()
        {
            if (uiController == null)
            {
                return;
            }

            uiController.SetScore(score);
            uiController.ShowStart();
        }

        private void ShowPlayingUI()
        {
            if (uiController == null)
            {
                return;
            }

            uiController.SetScore(score);
            uiController.ShowPlaying();
        }

        private void ShowGameOverUI()
        {
            if (uiController == null)
            {
                return;
            }

            uiController.SetScore(score);
            uiController.ShowGameOver();
        }

        private void RefreshScoreUI()
        {
            if (uiController != null)
            {
                uiController.SetScore(score);
            }
        }
    }
}
