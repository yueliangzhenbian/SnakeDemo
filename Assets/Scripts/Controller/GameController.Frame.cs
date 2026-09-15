using UnityEngine;

namespace SnakeGame
{
    public partial class GameController
    {
        private void FixedUpdate()
        {
            if (gameState != GameState.Playing)
            {
                return;
            }

            PlayingFrame(Time.fixedDeltaTime);
        }

        // private void Update()
        // {
        //     if (gameState != GameState.Playing)
        //     {
        //         return;
        //     }

        //     PlayingFrame(Time.deltaTime);
        // }

        private void PlayingFrame(float fixedDeltaTime)
        {
            snake.FrameUpdate(fixedDeltaTime);
            CheckPropCollision();
        }
    }
}
