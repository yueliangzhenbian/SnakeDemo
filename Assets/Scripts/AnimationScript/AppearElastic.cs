using System.Collections;
using UnityEngine;

namespace SnakeGame
{
    public sealed class AppearElastic : MonoBehaviour
    {
        private const float Duration = 0.45f;

        private Vector3 restScale = Vector3.one;
        private bool hasRestScale;
        private Coroutine routine;

        public static void Play(MonoBehaviour host)
        {
            if (host == null)
            {
                return;
            }

            AppearElastic anim = host.GetComponent<AppearElastic>();
            if (anim == null)
            {
                anim = host.gameObject.AddComponent<AppearElastic>();
            }

            anim.Begin();
        }

        public static void Release(MonoBehaviour host)
        {
            if (host == null)
            {
                return;
            }

            AppearElastic anim = host.GetComponent<AppearElastic>();
            if (anim != null)
            {
                anim.StopAndDestroy();
            }
        }

        private void Begin()
        {
            StopRunning();
            CaptureRestScale();
            if (!isActiveAndEnabled)
            {
                return;
            }

            routine = StartCoroutine(ScaleRoutine());
        }

        private void OnDisable()
        {
            StopRunning();
        }

        private void OnDestroy()
        {
            StopRunning();
        }

        private void StopAndDestroy()
        {
            StopRunning();
            Destroy(this);
        }

        private void StopRunning()
        {
            if (routine == null)
            {
                return;
            }

            StopCoroutine(routine);
            routine = null;
        }

        private void CaptureRestScale()
        {
            if (hasRestScale)
            {
                return;
            }

            restScale = transform.localScale;
            if (restScale.sqrMagnitude < 0.0001f)
            {
                restScale = Vector3.one;
            }

            hasRestScale = true;
        }

        private IEnumerator ScaleRoutine()
        {
            float time = 0f;
            transform.localScale = Vector3.zero;
            while (time < Duration)
            {
                if (this == null)
                {
                    yield break;
                }

                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / Duration);
                float k = Mathf.Max(0f, EaseOutElastic(t));
                transform.localScale = restScale * k;
                yield return null;
            }

            if (this == null)
            {
                yield break;
            }

            transform.localScale = restScale;
            routine = null;
            Destroy(this);
        }

        private static float EaseOutElastic(float t)
        {
            const float period = 0.3f;
            if (t <= 0f)
            {
                return 0f;
            }

            if (t >= 1f)
            {
                return 1f;
            }

            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - period / 4f) * (Mathf.PI * 2f) / period) + 1f;
        }
    }
}
