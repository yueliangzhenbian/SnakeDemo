using UnityEngine;

namespace SnakeGame
{
    //2D游戏,在XY平面移动
    public partial class GameController : MonoBehaviour
    {
        [SerializeField]
        private Snake snake;

        private int score;

        public int Score => score;

        private void Awake()
        {
            BindUI();
            ChangeState(GameState.Start);
        }

        private void OnDestroy()
        {
            UnbindUI();
        }

        public void AddScore(int value)
        {
            score += value;
            RefreshScoreUI();
        }
    }
}
