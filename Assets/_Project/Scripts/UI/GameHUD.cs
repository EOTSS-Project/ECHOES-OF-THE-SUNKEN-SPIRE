using UnityEngine;

// On-screen text in the voice of the Sunken Spire: water distance, height, saved crystals and short messages.
public class GameHUD : MonoBehaviour
{
    [SerializeField] private PlayerRespawn player;
    [SerializeField] private PlayerCarry carry;
    [SerializeField] private RisingWater water;

    private string message;
    private float messageUntil;
    private bool wasCarrying;
    private bool won;
    private int saved;
    private int total;
    private GUIStyle infoStyle;
    private GUIStyle messageStyle;

    public int Saved => saved;

    void Start()
    {
        total = FindObjectsByType<Crystal>(FindObjectsSortMode.None).Length;
        Show("Batık Kule uyanıyor... Sular yükselmeden tırman!", 4f);
    }

    void OnEnable()
    {
        GameEvents.PlayerDied += OnDied;
        GameEvents.CheckpointReached += OnCheckpoint;
        GameEvents.CrystalDeposited += OnDeposited;
        GameEvents.Won += OnWon;
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= OnDied;
        GameEvents.CheckpointReached -= OnCheckpoint;
        GameEvents.CrystalDeposited -= OnDeposited;
        GameEvents.Won -= OnWon;
    }

    void Update()
    {
        if (carry == null) return;
        bool carrying = carry.IsCarrying();
        if (carrying && !wasCarrying) Show("Bir ışık kristali buldun... Onu yeşil bir sığınağa taşı.", 3f);
        wasCarrying = carrying;
    }

    void OnDied(string reason)
    {
        Show(reason == "burned"
            ? "Kızıl mühür tenini yaktı... Son sığınakta gözlerini açıyorsun."
            : "Batık sular seni yuttu... Son sığınakta gözlerini açıyorsun.", 3f);
    }

    void OnCheckpoint() => Show("Yeşil sığınak: Kule bu adımını hatırlayacak.", 2f);

    void OnDeposited()
    {
        saved++;
        Show("Kristal sığınağa bırakıldı. Kulenin yankısı güçleniyor (" + saved + "/" + total + ")", 3f);
    }

    void OnWon()
    {
        won = true;
        if (saved >= total) Show("Bütün ışıkları kurtardın! Batık Kule yeniden parlıyor. (" + saved + "/" + total + ")", 999f);
        else if (saved > 0) Show("Zirveye ulaştın. Kurtardığın " + saved + " ışık kulenin yankısında yaşayacak. (" + saved + "/" + total + ")", 999f);
        else Show("Zirveye ulaştın... ama kule karanlıkta kaldı. (0/" + total + ")", 999f);
    }

    void Show(string text, float seconds)
    {
        message = text;
        messageUntil = Time.time + seconds;
    }

    void OnGUI()
    {
        if (infoStyle == null)
        {
            infoStyle = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
            messageStyle = new GUIStyle(GUI.skin.label) { fontSize = 30, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, wordWrap = true };
        }

        if (player != null && water != null && !won)
        {
            float gap = Mathf.Max(0f, player.FeetY - water.TopY);
            Line(new Rect(20, 15, 700, 32), gap < 3f ? "Sular ayağının dibinde: " + gap.ToString("0.0") + " m!" : "Sular: " + gap.ToString("0.0") + " m aşağıda",
                 gap < 3f ? new Color(1f, 0.4f, 0.35f) : new Color(0.6f, 0.85f, 1f), infoStyle);
            Line(new Rect(20, 45, 700, 32), "Yükseklik: " + Mathf.Max(0f, player.FeetY).ToString("0") + " m", Color.white, infoStyle);
            Line(new Rect(20, 75, 700, 32), "Işık Kristalleri: " + saved + " / " + total, new Color(1f, 0.9f, 0.3f), infoStyle);
            if (carry != null && carry.IsCarrying())
                Line(new Rect(20, 105, 700, 32), "Elinde bir kristal parlıyor", new Color(1f, 0.9f, 0.3f), infoStyle);
        }

        if (Time.time < messageUntil && !string.IsNullOrEmpty(message))
            Line(new Rect(Screen.width * 0.1f, Screen.height * 0.3f, Screen.width * 0.8f, 90), message,
                 won ? new Color(0.45f, 1f, 0.55f) : Color.white, messageStyle);
    }

    // Text with a dark shadow so it stays readable over the bright sky
    static void Line(Rect r, string text, Color color, GUIStyle style)
    {
        style.normal.textColor = new Color(0f, 0f, 0f, 0.8f);
        GUI.Label(new Rect(r.x + 2, r.y + 2, r.width, r.height), text, style);
        style.normal.textColor = color;
        GUI.Label(r, text, style);
    }
}
