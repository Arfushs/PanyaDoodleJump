using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _panyaGame.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private TextMeshProUGUI playerHeightText;
        [SerializeField] private RectTransform gameFailedScreen;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private Image fadeOutImage;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            playerHeightText.text = GameManager.Instance.CurrentHeight.ToString("F1") + "m";

        }

        public void ShowGameFailedScreen()
        {
            highScoreText.text = GameManager.Instance.CurrentHeight.ToString("F1") + "m";
            gameFailedScreen.DOAnchorPosY(0f, 1.5f);
            
        }

        public void Fade(Action onComplete = null)
        {
            fadeOutImage.DOFade(1f, 0.5f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    DOTween.KillAll();
                    onComplete?.Invoke(); // 🔹 Fade tamamlanınca çağrılır
                });
        }

    }
}
