using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeView : MonoBehaviour
    {

        #region 原始节点

        [SerializeField]
        private SnakeNode origionHeadNode;

        [SerializeField]
        private SnakeNode origionTailNode;

        [SerializeField]
        private SnakeNode origionBodyNode;

        #endregion

        private readonly List<SnakeNode> spawnedNodes = new List<SnakeNode>();

        // 蛇头走过的xy路径，下标越大越新，最后一个点是当前蛇头位置。
        private readonly List<Vector2> pathPoints = new List<Vector2>();

        private SnakeNode headNode;

        public SnakeNode HeadNode => headNode;

        public void ResetView(int bodyCount)
        {
            ClearSpawnedNodes();

            int count = Mathf.Max(0, bodyCount);
            Vector3 spacing = new Vector3(
                0f,
                -SnakeConstant.SNAKE_NODE_NORMAL_DISTANCE,
                SnakeConstant.SNAKE_NODE_Z_OFFSET);

            headNode = SpawnNode(origionHeadNode, Vector3.zero, "Head");

            SnakeNode previous = headNode;
            for (int i = 0; i < count; i++)
            {
                SnakeNode body = SpawnNode(origionBodyNode, spacing * (i + 1), $"Body_{i}");
                LinkNodes(previous, body);
                previous = body;
            }

            SnakeNode tail = SpawnNode(origionTailNode, spacing * (count + 1), "Tail");
            LinkNodes(previous, tail);

            InitPathFromNodes();
        }

        public void FollowNodes()
        {
            if (headNode == null)
            {
                return;
            }

            RecordHeadPathPoint();

            SnakeNode current = headNode.NextNode;
            float distanceAlongPath = 0f;
            int nodeIndex = 1;
            while (current != null)
            {
                distanceAlongPath += SnakeConstant.SNAKE_NODE_NORMAL_DISTANCE;
                PlaceNodeOnPath(current, distanceAlongPath, nodeIndex);
                current.UpdateDirLink();
                current = current.NextNode;
                nodeIndex++;
            }

            TrimPath(distanceAlongPath + SnakeConstant.SNAKE_NODE_NORMAL_DISTANCE);
        }

        public void AddNode()
        {
            if (headNode == null || origionBodyNode == null)
            {
                return;
            }

            SnakeNode oldNext = headNode.NextNode;
            Vector3 worldPos = oldNext != null ? oldNext.transform.position : headNode.transform.position;
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            SnakeNode body = SpawnNode(origionBodyNode, localPos, "Body", 1);

            headNode.LinkToNextNode(body);
            body.LinkToPreviousNode(headNode);
            if (oldNext != null)
            {
                body.LinkToNextNode(oldNext);
                oldNext.LinkToPreviousNode(body);
            }

            body.PlayAppear();
        }

        private void InitPathFromNodes()
        {
            pathPoints.Clear();
            SnakeNode node = headNode;
            while (node != null)
            {
                Vector3 pos = node.transform.position;
                pathPoints.Add(new Vector2(pos.x, pos.y));
                node = node.NextNode;
            }

            pathPoints.Reverse();
        }

        private void RecordHeadPathPoint()
        {
            Vector3 headPos = headNode.transform.position;
            Vector2 headXY = new Vector2(headPos.x, headPos.y);
            if (pathPoints.Count == 0)
            {
                pathPoints.Add(headXY);
                return;
            }

            Vector2 newest = pathPoints[pathPoints.Count - 1];
            if ((headXY - newest).sqrMagnitude <= 0.0001f)
            {
                pathPoints[pathPoints.Count - 1] = headXY;
                return;
            }

            pathPoints.Add(headXY);
        }

        private void PlaceNodeOnPath(SnakeNode node, float distanceAlongPath, int nodeIndex)
        {
            if (!TryGetPointAlongPath(distanceAlongPath, out Vector2 posXY, out Vector2 tangent))
            {
                return;
            }

            Vector3 pos = node.transform.position;
            pos.x = posXY.x;
            pos.y = posXY.y;
            pos.z = headNode.transform.position.z + SnakeConstant.SNAKE_NODE_Z_OFFSET * nodeIndex;
            node.transform.position = pos;

            if (tangent.sqrMagnitude > 0.0001f)
            {
                SnakeNode.SetPlanarRotation(node.transform, tangent);
            }
        }

        // 从蛇头沿历史路径往回走 distance 的xy弧长，不限制欧氏最小距离，盘起来时节点可以靠得很近。
        private bool TryGetPointAlongPath(float distance, out Vector2 position, out Vector2 tangent)
        {
            position = default;
            tangent = Vector2.up;

            int count = pathPoints.Count;
            if (count == 0)
            {
                return false;
            }

            if (count == 1 || distance <= 0f)
            {
                position = pathPoints[count - 1];
                if (count >= 2)
                {
                    tangent = pathPoints[count - 1] - pathPoints[count - 2];
                }

                return true;
            }

            float remaining = distance;
            for (int i = count - 1; i > 0; i--)
            {
                Vector2 newer = pathPoints[i];
                Vector2 older = pathPoints[i - 1];
                Vector2 toOlder = older - newer;
                float segmentLength = toOlder.magnitude;
                if (segmentLength <= 0.0001f)
                {
                    continue;
                }

                if (remaining <= segmentLength)
                {
                    position = newer + toOlder * (remaining / segmentLength);
                    tangent = -toOlder;
                    return true;
                }

                remaining -= segmentLength;
            }

            position = pathPoints[0];
            if (count >= 2)
            {
                tangent = pathPoints[1] - pathPoints[0];
            }

            return true;
        }

        private void TrimPath(float keepDistance)
        {
            int count = pathPoints.Count;
            if (count <= 2)
            {
                return;
            }

            float accumulated = 0f;
            int oldestKeepIndex = 0;
            for (int i = count - 1; i > 0; i--)
            {
                accumulated += (pathPoints[i] - pathPoints[i - 1]).magnitude;
                if (accumulated >= keepDistance)
                {
                    oldestKeepIndex = i - 1;
                    break;
                }
            }

            if (oldestKeepIndex > 0)
            {
                pathPoints.RemoveRange(0, oldestKeepIndex);
            }
        }

        private SnakeNode SpawnNode(SnakeNode prefab, Vector3 localPosition, string nodeName, int insertIndex = -1)
        {
            SnakeNode node = Instantiate(prefab, transform);
            node.name = nodeName;
            node.transform.localPosition = localPosition;
            node.transform.localRotation = Quaternion.identity;
            if (insertIndex < 0 || insertIndex >= spawnedNodes.Count)
            {
                spawnedNodes.Add(node);
            }
            else
            {
                spawnedNodes.Insert(insertIndex, node);
            }

            return node;
        }

        private static void LinkNodes(SnakeNode previous, SnakeNode next)
        {
            previous.LinkToNextNode(next);
            next.LinkToPreviousNode(previous);
        }

        private void ClearSpawnedNodes()
        {
            for (int i = 0; i < spawnedNodes.Count; i++)
            {
                if (spawnedNodes[i] != null)
                {
                    spawnedNodes[i].ReleaseAppear();
                    Destroy(spawnedNodes[i].gameObject);
                }
            }

            spawnedNodes.Clear();
            pathPoints.Clear();
            headNode = null;
        }
    }
}
