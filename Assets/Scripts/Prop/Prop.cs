using UnityEngine;

namespace SnakeGame
{
    public enum PropType
    {
        Node = 0,
        SnakeBody = 1
    }

    public class Prop : MonoBehaviour
    {
        [SerializeField]
        private PropType propType;

        [SerializeField]
        private float colliderRadius = 0.4f;

        [SerializeField]
        private float weight = 1f;//权重，用于随机生成

        public PropType PropType => propType;

        public float ColliderRadius => colliderRadius;

        public float Weight => weight;

        public void PlayAppear()
        {
            AppearElastic.Play(this);
        }

        public void ReleaseAppear()
        {
            AppearElastic.Release(this);
        }

        private void OnDisable()
        {
            ReleaseAppear();
        }

        public bool IsHit(Vector2 point, float otherRadius)
        {
            Vector3 pos = transform.position;
            Vector2 propXY = new Vector2(pos.x, pos.y);
            float radius = colliderRadius + otherRadius;
            return (propXY - point).sqrMagnitude <= radius * radius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 pos = transform.position;
            Gizmos.DrawWireSphere(new Vector3(pos.x, pos.y, pos.z), colliderRadius);
        }
    }
}
