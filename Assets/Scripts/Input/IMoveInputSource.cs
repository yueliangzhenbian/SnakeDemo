using UnityEngine;

namespace SnakeGame
{
    public interface IMoveInputSource
    {
        Vector2 GetRawInput();
    }
}
