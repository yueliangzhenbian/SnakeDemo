using UnityEngine;

namespace SnakeGame
{
    public sealed class SnakeMoveInput
    {
        private IMoveInputSource extraSource;

        public void SetExtraSource(IMoveInputSource source)
        {
            extraSource = source;
        }

        public void Apply(ref bool hasMoveInput, ref Vector2 pendingDirection, Vector2 currentMoveDirection)
        {
            Vector2 raw = KeyboardMoveInput.ReadRaw();
            if (extraSource != null)
            {
                raw += extraSource.GetRawInput();
            }

            if (raw.sqrMagnitude < 0.0001f)
            {
                hasMoveInput = false;
                return;
            }

            raw.Normalize();
            if (currentMoveDirection.sqrMagnitude > 0.0001f && Vector2.Dot(raw, currentMoveDirection) < -0.99f)
            {
                return;
            }

            hasMoveInput = true;
            pendingDirection = raw;
        }
    }
}
