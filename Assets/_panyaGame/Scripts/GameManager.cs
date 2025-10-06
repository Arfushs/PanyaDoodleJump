using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Transform player;
    public float CurrentHeight { get; private set; } = 0;
    private float initialPlayerHeight;

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
        CurrentHeight = player.position.y -  initialPlayerHeight;
    }

    private void OnGUI()
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            normal = { textColor = Color.white }
        };

        // hafif gölge için arka plan yazısı
        var shadowStyle = new GUIStyle(style);
        shadowStyle.normal.textColor = new Color(0f, 0f, 0f, 0.6f);

        string text = $"Height: {CurrentHeight:F1}";

        // konum
        Rect rect = new Rect(10, 10, 250, 30);

        // gölge
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), text, shadowStyle);
        // asıl yazı
        GUI.Label(rect, text, style);
    }
}
