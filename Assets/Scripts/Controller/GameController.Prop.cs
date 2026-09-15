using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public partial class GameController
    {
        [SerializeField]
        private Prop[] origionProps;

        [SerializeField]
        private Transform propsParent;

        private readonly List<Prop> spawnedProps = new List<Prop>();
        private int eatenPropCount;

        private void ResetProps()
        {
            eatenPropCount = 0;
            ClearSpawnedProps();
            int spawnCount = Random.Range(SnakeConstant.PROP_COUNT_MIN, SnakeConstant.PROP_COUNT_MAX + 1);
            SpawnProps(spawnCount);
        }

        private void ClearSpawnedProps()
        {
            for (int i = 0; i < spawnedProps.Count; i++)
            {
                if (spawnedProps[i] != null)
                {
                    spawnedProps[i].ReleaseAppear();
                    Destroy(spawnedProps[i].gameObject);
                }
            }

            spawnedProps.Clear();
        }

        private void ReplenishProps()
        {
            int current = CountAliveProps();
            int target = Random.Range(SnakeConstant.PROP_COUNT_MIN, SnakeConstant.PROP_COUNT_MAX + 1);
            if (target > current)
            {
                SpawnProps(target - current);
            }
        }

        private void CheckPropCollision()
        {
            if (snake == null || !snake.TryGetHeadCollision(out Vector2 headXY, out float headRadius))
            {
                return;
            }

            for (int i = spawnedProps.Count - 1; i >= 0; i--)
            {
                Prop prop = spawnedProps[i];
                if (prop == null)
                {
                    spawnedProps.RemoveAt(i);
                    continue;
                }

                if (!prop.IsHit(headXY, headRadius))
                {
                    continue;
                }

                OnPropEaten(prop);
                if (gameState != GameState.Playing)
                {
                    return;
                }
            }
        }

        private void OnPropEaten(Prop prop)
        {
            snake.ApplyProp(prop);
            spawnedProps.Remove(prop);
            prop.ReleaseAppear();
            Destroy(prop.gameObject);

            eatenPropCount++;
            AddScore(1);

            if (eatenPropCount >= SnakeConstant.PROP_EAT_TO_FINISH)
            {
                GameOver();
                return;
            }

            ReplenishProps();
        }

        private void SpawnProps(int count)
        {
            if (origionProps == null || origionProps.Length == 0 || count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Prop prefab = PickPropPrefab();
                if (prefab == null)
                {
                    continue;
                }

                if (!TryFindSpawnPosition(prefab.ColliderRadius, out Vector2 position))
                {
                    continue;
                }

                Transform parent = propsParent != null ? propsParent : transform;
                Prop prop = Instantiate(prefab, new Vector3(position.x, position.y, 0f), Quaternion.identity, parent);
                spawnedProps.Add(prop);
                prop.PlayAppear();
            }
        }

        private Prop PickPropPrefab()
        {
            float totalWeight = 0f;
            for (int i = 0; i < origionProps.Length; i++)
            {
                if (origionProps[i] != null)
                {
                    totalWeight += Mathf.Max(0f, origionProps[i].Weight);
                }
            }

            if (totalWeight <= 0f)
            {
                return origionProps[Random.Range(0, origionProps.Length)];
            }

            float pick = Random.Range(0f, totalWeight);
            float accumulated = 0f;
            for (int i = 0; i < origionProps.Length; i++)
            {
                Prop prefab = origionProps[i];
                if (prefab == null)
                {
                    continue;
                }

                accumulated += Mathf.Max(0f, prefab.Weight);
                if (pick <= accumulated)
                {
                    return prefab;
                }
            }

            return origionProps[origionProps.Length - 1];
        }

        private bool TryFindSpawnPosition(float radius, out Vector2 position)
        {
            float xMin = SnakeConstant.MOVE_BOUND_X_MIN + radius;
            float xMax = SnakeConstant.MOVE_BOUND_X_MAX - radius;
            float yMin = SnakeConstant.MOVE_BOUND_Y_MIN + radius;
            float yMax = SnakeConstant.MOVE_BOUND_Y_MAX - radius;

            for (int attempt = 0; attempt < SnakeConstant.PROP_SPAWN_MAX_ATTEMPTS; attempt++)
            {
                Vector2 candidate = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
                if (!OverlapsExistingProp(candidate, radius) && !OverlapsSnakeHead(candidate, radius))
                {
                    position = candidate;
                    return true;
                }
            }

            position = default;
            return false;
        }

        private bool OverlapsExistingProp(Vector2 position, float radius)
        {
            for (int i = 0; i < spawnedProps.Count; i++)
            {
                Prop prop = spawnedProps[i];
                if (prop == null)
                {
                    continue;
                }

                Vector3 pos = prop.transform.position;
                Vector2 propXY = new Vector2(pos.x, pos.y);
                float minDistance = radius + prop.ColliderRadius;
                if ((position - propXY).sqrMagnitude < minDistance * minDistance)
                {
                    return true;
                }
            }

            return false;
        }

        private bool OverlapsSnakeHead(Vector2 position, float radius)
        {
            if (snake == null || !snake.TryGetHeadCollision(out Vector2 headXY, out float headRadius))
            {
                return false;
            }

            float minDistance = radius + headRadius;
            return (position - headXY).sqrMagnitude < minDistance * minDistance;
        }

        private int CountAliveProps()
        {
            int count = 0;
            for (int i = spawnedProps.Count - 1; i >= 0; i--)
            {
                if (spawnedProps[i] == null)
                {
                    spawnedProps.RemoveAt(i);
                    continue;
                }

                count++;
            }

            return count;
        }
    }
}
