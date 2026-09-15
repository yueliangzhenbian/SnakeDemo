using System;
using UnityEngine;
using UnityEngine.UI;

namespace SnakeGame
{
    public class StartView : UIView
    {
        [SerializeField]
        private Button startButton;

        public event Action OnStartClicked;

        private void Awake()
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
            }
        }
    }
}
