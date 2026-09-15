using UnityEngine;
using UnityEngine.EventSystems;

namespace SnakeGame
{
    public class JoystickView : MonoBehaviour, IMoveInputSource, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private RectTransform handle;

        [SerializeField]
        private float handleRange = 100f;

        private RectTransform root;
        private Vector2 direction;

        public Vector2 Direction => direction;

        private void Awake()
        {
            root = transform as RectTransform;
        }

        private void OnDisable()
        {
            ResetHandle();
        }

        public Vector2 GetRawInput()
        {
            return direction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (root == null)
            {
                return;
            }

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                root,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint);

            Vector2 clamped = Vector2.ClampMagnitude(localPoint, handleRange);
            if (handle != null)
            {
                handle.anchoredPosition = clamped;
            }

            direction = handleRange > 0.0001f ? clamped / handleRange : Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ResetHandle();
        }

        private void ResetHandle()
        {
            direction = Vector2.zero;
            if (handle != null)
            {
                handle.anchoredPosition = Vector2.zero;
            }
        }
    }
}
