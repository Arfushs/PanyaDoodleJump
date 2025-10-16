using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _panyaGame.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private Image fadeOutImage;
        private void Start()
        {
            highScoreText.text = PlayerPrefs.GetInt("highScore", 0).ToString() + "m";
        }

        public void OnPlayButtonPressed()
        {
            fadeOutImage.DOFade(1f, 0.5f)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true) // sahne geçişi sırasında bile çalışsın
                .OnComplete(() =>
                {
                    DOTween.KillAll(); // aktif tweenleri temizle (önlem amaçlı)
                    SceneManager.LoadScene("GameScene");
                });
        
        }
    }
}
