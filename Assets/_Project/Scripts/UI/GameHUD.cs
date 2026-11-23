using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    public static GameHUD Instance;

    [SerializeField] private PlayerRespawn player;
    [SerializeField] private PlayerCarry carry;
    [SerializeField] private RisingWater water;
    [SerializeField] private Round round;

    private string message;
    private float messageUntil;
    private bool wasCarrying;

    // Oyun durumları
    private bool gameStarted = false;
    private bool won = false;
    private bool lost = false;

    // Ölüm ve Su Başlangıç Zamanı Takibi
    private int deathCount = 0;
    private const int MaxDeaths = 5;
    private float gameStartTime = 0f;
    private const float waterStartDelay = 6f;
    private Vector3 initialCheckpoint; // Başlangıç noktasını hafızada tutmak için eklendi

    private int savedLight = 0;
    private int totalLight = 4;

    private int savedHeavy = 0;
    private int totalHeavy = 3;

    private int currentScore = 0;
    private const int MaxScore = 9;

    private GUIStyle infoStyle;
    private GUIStyle messageStyle;
    private GUIStyle winTitleStyle;
    private GUIStyle winScoreStyle;
    private GUIStyle subtitleStyle;
    private GUIStyle loseTitleStyle;
    private GUIStyle loseScoreStyle;
    private GUIStyle countdownStyle;
    private GUIStyle restartBtnStyle;

    private Texture2D purpleTex;
    private Texture2D blackTex;

    public int Saved => savedLight + savedHeavy;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 0f;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Oyun ilk başladığındaki doğma noktasını kaydet
        if (player != null)
        {
            initialCheckpoint = player.CheckpointFeet;
        }

        Show("Batık Kule uyanıyor... Sular yükselmeden tırman!", 4f);
    }

    void OnEnable()
    {
        GameEvents.PlayerDied += OnDied;
        GameEvents.CheckpointReached += OnCheckpoint;
        GameEvents.CrystalLost += OnLost;
        GameEvents.Hint += OnHint;
        GameEvents.FinalRush += OnFinalRush;
        GameEvents.Won += OnWon;
        GameEvents.PlayerRespawned += OnRespawned; // Yeniden doğma event'ini dinle
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= OnDied;
        GameEvents.CheckpointReached -= OnCheckpoint;
        GameEvents.CrystalLost -= OnLost;
        GameEvents.Hint -= OnHint;
        GameEvents.FinalRush -= OnFinalRush;
        GameEvents.Won -= OnWon;
        GameEvents.PlayerRespawned -= OnRespawned;
    }

    void OnHint(string text) => Show(text, 5f);
    void OnFinalRush() => Show("Kule sarsılıyor! Sular kabarıyor... Zirveye koş!", 4f);
    void OnLost() => Show("Bir ışık sulara gömüldü... Kule onu bir daha geri vermeyecek.", 3f);

    void Update()
    {
        if (!gameStarted || carry == null || won || lost) return;

        bool carrying = carry.IsCarrying();
        if (carrying && !wasCarrying) Show("Bir ışık kristali buldun... Onu yeşil bir sığınağa taşı.", 3f);
        wasCarrying = carrying;
    }

    void OnDied(string reason)
    {
        deathCount++;
        if (deathCount >= MaxDeaths)
        {
            TriggerLose(); // 5. ölümde oyunu bitir
        }
        else
        {
            Show(reason == "burned"
                ? "Kızıl mühür tenini yaktı... Son sığınakta gözlerini açıyorsun."
                : "Batık sular seni yuttu... Son sığınakta gözlerini açıyorsun.", 3f);
        }
    }

    void OnRespawned(Vector3 checkpointFeet)
    {
        // Sadece ilk doğduğumuz noktada (Yükseklikleri eşleşiyorsa) sayacı sıfırla
        if (Mathf.Abs(checkpointFeet.y - initialCheckpoint.y) < 1f)
        {
            gameStartTime = Time.time;
        }
    }

    void OnCheckpoint() => Show("Yeşil sığınak: Kule bu adımını hatırlayacak.", 2f);

    public void RegisterDeposit(bool isHeavy, int points)
    {
        if (isHeavy)
        {
            savedHeavy++;
            int added = points > 0 ? points : 2;
            currentScore += added;
            if (round != null) round.AddPoints(added);
            Show($"Ağır ışık kristali sığınağa bırakıldı! (+{added} Puan)", 3f);
        }
        else
        {
            savedLight++;
            int added = points > 0 ? points : 1;
            currentScore += added;
            Show($"Hafif ışık kristali sığınağa bırakıldı! (+{added} Puan)", 3f);
        }
    }

    public void TriggerWin()
    {
        if (won || lost) return;
        won = true;
        message = "";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        if (round != null)
           round.EndRound("Zirveye ulaştın! Kule ışığına kavuştu.");
    }

    public void TriggerLose()
    {
        if (won || lost) return;
        lost = true;
        message = "";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Ölünce de zamanı ve hareketi durdur
        if (round != null)
            round.EndRound("Batık sular seni yuttu...");
    }

    void OnWon()
    {
        TriggerWin();
    }

    void Show(string text, float seconds)
    {
        if (won || lost) return;
        message = text;
        messageUntil = Time.unscaledTime + seconds;
    }

    void InitStyles()
    {
        if (infoStyle == null)
        {
            infoStyle = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
            messageStyle = new GUIStyle(GUI.skin.label) { fontSize = 30, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, wordWrap = true };
            winTitleStyle = new GUIStyle(GUI.skin.label) { fontSize = 44, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            winScoreStyle = new GUIStyle(GUI.skin.label) { fontSize = 36, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };

            // Yeni Eklenen Yazı Stilleri
            subtitleStyle = new GUIStyle(GUI.skin.label) { fontSize = 18, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            loseTitleStyle = new GUIStyle(GUI.skin.label) { fontSize = 48, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            loseScoreStyle = new GUIStyle(GUI.skin.label) { fontSize = 38, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };

            // Su geri sayımı fontu (Orta alt, büyük)
            countdownStyle = new GUIStyle(GUI.skin.label) { fontSize = 36, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
        }

        if (purpleTex == null)
        {
            purpleTex = new Texture2D(1, 1);
            purpleTex.SetPixel(0, 0, new Color(0.45f, 0.15f, 0.75f, 1f));
            purpleTex.Apply();

            restartBtnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 26,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            restartBtnStyle.normal.background = purpleTex;
            restartBtnStyle.hover.background = purpleTex;
            restartBtnStyle.active.background = purpleTex;
            restartBtnStyle.normal.textColor = Color.white;
            restartBtnStyle.hover.textColor = new Color(0.9f, 0.9f, 0.9f);
            restartBtnStyle.active.textColor = Color.white;
        }

        if (blackTex == null)
        {
            blackTex = new Texture2D(1, 1);
            blackTex.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.85f));
            blackTex.Apply();
        }
    }

    void OnGUI()
    {
        InitStyles();
        Color yellowColor = new Color(1f, 0.9f, 0.2f);
        Color orangeColor = new Color(1f, 0.55f, 0.1f);
        Color blueColor = new Color(0.2f, 0.7f, 1f);

        // 1. BAŞLANGIÇ EKRANI
        if (!gameStarted)
        {
            float btnWidth = 240f;
            float btnHeight = 58f;
            float btnX = (Screen.width - btnWidth) * 0.5f;
            float btnY = (Screen.height - btnHeight) * 0.5f;

            if (GUI.Button(new Rect(btnX, btnY, btnWidth, btnHeight), "OYUNA BAŞLA", restartBtnStyle))
            {
                gameStarted = true;
                gameStartTime = Time.time; // Oyuna tıklandığı anı kaydet
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            string subText = "Kristalleri taşı, parkuru tamamla ve en yüksek puanı topla! 5 kez ölürsen oyunu kaybedersin!";
            Vector2 subSize = subtitleStyle.CalcSize(new GUIContent(subText));
            float subX = (Screen.width - subSize.x) * 0.5f;
            float subY = btnY + btnHeight + 25f;

            GUI.DrawTexture(new Rect(subX - 15, subY - 10, subSize.x + 30, subSize.y + 20), blackTex);
            Line(new Rect(subX, subY, subSize.x, subSize.y), subText, yellowColor, subtitleStyle);

            return;
        }
        
        // Bitiş ekranını artık GameOver paneli gösteriyor
        if (round != null && round.IsOver) return;

        // 2. OYUN İÇİ HUD
        if (player != null && water != null && !won && !lost)
        {
            float gap = Mathf.Max(0f, player.FeetY - water.TopY);
            if (!water.IsRising)
                Line(new Rect(20, 15, 700, 32), "Sular henüz uyuyor... (" + gap.ToString("0.0") + " m aşağıda)", new Color(0.6f, 0.85f, 1f), infoStyle);
            else
                Line(new Rect(20, 15, 700, 32), gap < 3f ? "Sular ayağının dibinde: " + gap.ToString("0.0") + " m!" : "Sular: " + gap.ToString("0.0") + " m aşağıda",
                     gap < 3f ? new Color(1f, 0.4f, 0.35f) : new Color(0.6f, 0.85f, 1f), infoStyle);

            Line(new Rect(20, 45, 700, 32), "Yükseklik: " + Mathf.Max(0f, player.FeetY).ToString("0") + " m", Color.white, infoStyle);
            Line(new Rect(20, 75, 700, 32), $"Hafif Işık Kristalleri: {savedLight} / {totalLight}", yellowColor, infoStyle);
            Line(new Rect(20, 105, 700, 32), $"Ağır Işık Kristalleri: {savedHeavy} / {totalHeavy}", orangeColor, infoStyle);

            Line(new Rect(20, 165, 700, 32), $"Ölüm Sayısı: {deathCount} / {MaxDeaths}", Color.red, infoStyle);

            if (carry != null && carry.IsCarrying() && carry.CarriedCrystal != null)
            {
                bool isHeavy = carry.CarriedCrystal.Weight() > 1f || (carry.CarriedCrystal.Type != null && carry.CarriedCrystal.Type.name.ToLower().Contains("heavy"));
                Color carryColor = isHeavy ? orangeColor : yellowColor;
                Line(new Rect(20, 195, 700, 32), "Elinde bir kristal parlıyor", carryColor, infoStyle);
            }

            // --- ORTA ALT SU YÜKSELME GERİ SAYIMI ---
            float elapsed = Time.time - gameStartTime;
            float remaining = waterStartDelay - elapsed;
            float countdownY = Screen.height - 80f;

            if (remaining >= 0)
            {
                int displayTime = Mathf.CeilToInt(remaining); // 6, 5, 4...0
                Line(new Rect(0, countdownY, Screen.width, 50), $"Suyun yükselmeye başlamasına son {displayTime}", blueColor, countdownStyle);
            }
            else if (remaining >= -2f) // 0'ı geçtikten sonra TAM 2 SANİYE boyunca
            {
                // (-remaining % 1f) < 0.5f mantığı ile saniyede 1 kere, toplamda 2 kere yanıp söner
                if ((-remaining % 1f) < 0.5f)
                {
                    Line(new Rect(0, countdownY, Screen.width, 50), "Suyun yükselmeye başlamasına son 0", Color.red, countdownStyle);
                }
            }
        }

        // Oyun içi geçici bildirim mesajları
        if (!won && !lost && Time.unscaledTime < messageUntil && !string.IsNullOrEmpty(message))
        {
            Line(new Rect(Screen.width * 0.1f, Screen.height * 0.3f, Screen.width * 0.8f, 90), message, Color.white, messageStyle);
        }

        // 3. OYUN SONU (KAZANDIN) EKRANI
        if (won)
        {
            float titleY = Screen.height * 0.36f;
            float scoreY = titleY + 65f;

            float bgWidth = 700f;
            float bgHeight = 165f;
            float bgX = (Screen.width - bgWidth) * 0.5f;
            float bgY = titleY - 25f;

            GUI.DrawTexture(new Rect(bgX, bgY, bgWidth, bgHeight), blackTex);

            Line(new Rect(0, titleY, Screen.width, 60), "TEBRİKLER TAMAMLADINIZ!", Color.white, winTitleStyle);
            Line(new Rect(0, scoreY, Screen.width, 50), $"TOPLAM PUAN: {currentScore}/{MaxScore}", orangeColor, winScoreStyle);

            DrawRestartButton();
        }

        // 4. OYUN SONU (KAYBETTİN) EKRANI
        if (lost)
        {
            float titleY = Screen.height * 0.36f;
            float scoreY = titleY + 65f;

            float bgWidth = 450f;
            float bgHeight = 165f;
            float bgX = (Screen.width - bgWidth) * 0.5f;
            float bgY = titleY - 25f;

            GUI.DrawTexture(new Rect(bgX, bgY, bgWidth, bgHeight), blackTex);

            Line(new Rect(0, titleY, Screen.width, 60), "KAYBETTİN!", Color.red, loseTitleStyle);
            Line(new Rect(0, scoreY, Screen.width, 50), $"Ölüm Sayısı: {deathCount}/{MaxDeaths}", Color.red, loseScoreStyle);

            DrawRestartButton();
        }
    }

    // Hem kazanınca hem kaybedince çıkan Mor Butonun methodu
    private void DrawRestartButton()
    {
        float btnWidth = 240f;
        float btnHeight = 58f;
        float btnX = (Screen.width - btnWidth) * 0.5f;
        float btnY = Screen.height * 0.72f;

        if (GUI.Button(new Rect(btnX, btnY, btnWidth, btnHeight), "Tekrar Oyna", restartBtnStyle))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    static void Line(Rect r, string text, Color color, GUIStyle style)
    {
        style.normal.textColor = new Color(0f, 0f, 0f, 0.85f);
        GUI.Label(new Rect(r.x + 2, r.y + 2, r.width, r.height), text, style); // Gölge
        style.normal.textColor = color;
        GUI.Label(r, text, style); // Ana Yazı
    }
}