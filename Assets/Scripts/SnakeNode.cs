using UnityEngine;

namespace SnakeGame
{
    public enum SnakeNodeType
    {
        Head = 0, // 头部节点,没有前驱节点
        Body = 1, // 身体节点,有前驱节点和后继节点
        Tail = 2 // 尾部节点,没有后继节点
    }
    public class SnakeNode : MonoBehaviour
    {

        [SerializeField]
        private SnakeNodeType nodeType;

        [SerializeField]
        private SnakeNode nextNode;

        [SerializeField]
        private SnakeNode previousNode;

        [SerializeField]
        private Transform dirNode; //从当前节点指向上级节点

        public SnakeNodeType NodeType => nodeType;

        public SnakeNode NextNode => nextNode;

        public SnakeNode PreviousNode => previousNode;

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

        public void LinkToNextNode(SnakeNode nextNode)
        {
            this.nextNode = nextNode;
        }

        public void LinkToPreviousNode(SnakeNode previousNode)
        {
            this.previousNode = previousNode;
            UpdateDirLink();
        }

        public void UpdateDirLink()
        {
            if (dirNode == null)
            {
                return;
            }

            if (previousNode == null)
            {
                dirNode.gameObject.SetActive(false);
                return;
            }

            dirNode.gameObject.SetActive(true);

            Vector3 toPrev = previousNode.transform.position - transform.position;
            toPrev.z = 0f;
            float xyLength = toPrev.magnitude;
            if (xyLength >= 0.0001f)
            {
                SetPlanarRotation(dirNode, toPrev);
            }

            Vector3 scale = dirNode.localScale;
            scale.y = Mathf.Clamp(xyLength, 0.3f, 1f);
            dirNode.localScale = scale;
        }

        public static void SetPlanarRotation(Transform target, Vector2 direction)
        {
            if (target == null || direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            target.rotation = Quaternion.Euler(0f, 0f, zAngle);
        }

    }

}

