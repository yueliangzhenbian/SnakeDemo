namespace SnakeGame
{
    public enum GameState
    {
        None = 0,
        Start = 1,
        Playing = 2,
        GameOver = 3
    }

    public partial class GameController
    {
        private GameState gameState;

        public GameState GameState => gameState;

        private void ChangeState(GameState newState)
        {
            gameState = newState;
            switch (gameState)
            {
                case GameState.Start:
                    EnterStartState();
                    break;
                case GameState.Playing:
                    EnterPlayingState();
                    break;
                case GameState.GameOver:
                    EnterGameOverState();
                    break;
            }
        }

        public void StartGame()
        {
            ChangeState(GameState.Playing);
        }

        public void GameOver()
        {
            ChangeState(GameState.GameOver);
        }

        private void ReturnToStart()
        {
            ChangeState(GameState.Start);
        }

        private void EnterStartState()
        {
            score = 0;
            ClearFightScene();
            ShowStartUI();
        }

        private void EnterPlayingState()
        {
            score = 0;
            if (snake != null)
            {
                snake.Reset(SnakeConstant.DEFAULT_BODY_COUNT);
            }

            ResetProps();
            ShowPlayingUI();
        }

        private void EnterGameOverState()
        {
            ClearSpawnedProps();
            ShowGameOverUI();
        }

        private void ClearFightScene()
        {
            eatenPropCount = 0;
            ClearSpawnedProps();
            if (snake != null)
            {
                snake.Reset(SnakeConstant.DEFAULT_BODY_COUNT);
            }
        }
    }
}
