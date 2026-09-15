namespace SnakeGame
{
    public static class SnakeConstant
    {
        public const int DEFAULT_BODY_COUNT = 1;

        public const float SNAKE_NODE_NORMAL_DISTANCE = 0.9f; //节点在xy平面上的常态距离

        public const float SNAKE_NODE_Z_OFFSET = 0.1f; //每一级节点的z轴偏移，用于层级遮挡

        public const float SNAKE_MOVE_SPEED = 5f; //蛇头移动速度

        public const float MOVE_BOUND_X_MIN = -5f;
        public const float MOVE_BOUND_X_MAX = 5f;
        public const float MOVE_BOUND_Y_MIN = -9.3f;
        public const float MOVE_BOUND_Y_MAX = 9.3f;

        public const int PROP_COUNT_MIN = 2;
        public const int PROP_COUNT_MAX = 4;
        public const int PROP_EAT_TO_FINISH = 20;
        public const int PROP_SPAWN_MAX_ATTEMPTS = 24;
    }
}