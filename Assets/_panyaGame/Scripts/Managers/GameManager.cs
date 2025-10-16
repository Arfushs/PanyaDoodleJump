using System;
using _panyaGame.Scripts.Player_Related;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _panyaGame.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private Transform player;
        public float CurrentHeight { get; private set; } = 0;
        public float LastHeight { get; private set; } = 0;
        private float initialPlayerHeight;

        private void OnEnable()
        {
            PlayerController.OnPlayerLost += OnPlayerDeath;
        }
        

        private void OnDisable()
        {
            PlayerController.OnPlayerLost -= OnPlayerDeath;
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            initialPlayerHeight = player.position.y;
        }

        private void Update()
        {
            UpdateCurrentHeight();
        }

        private void UpdateCurrentHeight()
        {
            LastHeight = player.position.y -  initialPlayerHeight;
            if(LastHeight > CurrentHeight)
                CurrentHeight = LastHeight;
        }
        
        private void OnPlayerDeath()
        {
            PlayerPrefs.SetInt("highScore", (int)CurrentHeight);
            UIManager.Instance.ShowGameFailedScreen();
        }

        public void OnRestarButtonPressed()
        {
            UIManager.Instance.Fade(() =>
            {
                SceneManager.LoadScene("GameScene");
            });
        }

        
    }
}
