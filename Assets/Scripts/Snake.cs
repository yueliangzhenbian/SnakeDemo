using UnityEngine;

namespace SnakeGame
{
    public class Snake : MonoBehaviour
    {
        [SerializeField]
        private SnakeView snakeView;

        [SerializeField]
        private float snakeHeadColliderRadius = 0.5f;

        private readonly SnakeMoveInput moveInput = new SnakeMoveInput();

        private Vector2 moveDirection = Vector2.up;
        private Vector2 pendingDirection = Vector2.up;
        private bool hasMoveInput;

        public void SetMoveExtraSource(IMoveInputSource source)
        {
            moveInput.SetExtraSource(source);
        }

        public void Reset(int bodyCount)
        {
            moveDirection = Vector2.up;
            pendingDirection = Vector2.up;
            hasMoveInput = false;
            snakeView.ResetView(bodyCount);
        }

        private void Update()
        {
            moveInput.Apply(ref hasMoveInput, ref pendingDirection, moveDirection);
        }

        public void FrameUpdate(float fixedDeltaTime)
        {
            if (snakeView == null || snakeView.HeadNode == null)
            {
                return;
            }

            if (!hasMoveInput)
            {
                return;
            }

            moveDirection = pendingDirection;

            SnakeNode head = snakeView.HeadNode;
            Vector3 movement = new Vector3(moveDirection.x, moveDirection.y, 0f)
                               * SnakeConstant.SNAKE_MOVE_SPEED
                               * fixedDeltaTime;
            head.transform.position += movement;
            ClampHeadToBounds(head);
            SnakeNode.SetPlanarRotation(head.transform, moveDirection);

            snakeView.FollowNodes();
        }

        public bool TryGetHeadCollision(out Vector2 headXY, out float radius)
        {
            radius = snakeHeadColliderRadius;
            if (snakeView == null || snakeView.HeadNode == null)
            {
                headXY = default;
                return false;
            }

            Vector3 headPos = snakeView.HeadNode.transform.position;
            headXY = new Vector2(headPos.x, headPos.y);
            return true;
        }

        public void ApplyProp(Prop prop)
        {
            if (prop == null)
            {
                return;
            }

            switch (prop.PropType)
            {
                case PropType.SnakeBody:
                    AddNode();
                    break;
                case PropType.Node:
                    break;
            }
        }

        public void AddNode()
        {
            if (snakeView != null)
            {
                snakeView.AddNode();
            }
        }

        private static void ClampHeadToBounds(SnakeNode head)
        {
            Vector3 pos = head.transform.position;
            pos.x = Mathf.Clamp(pos.x, SnakeConstant.MOVE_BOUND_X_MIN, SnakeConstant.MOVE_BOUND_X_MAX);
            pos.y = Mathf.Clamp(pos.y, SnakeConstant.MOVE_BOUND_Y_MIN, SnakeConstant.MOVE_BOUND_Y_MAX);
            head.transform.position = pos;
        }
    }
}
