using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private enum MenuState { Home, HowToPlay }

    private static bool hasPlayedOnce = false;
    private bool isMenuOpen = true;
    private MenuState currentState = MenuState.Home;

    private Texture2D backgroundTexture;
    private Font serifFont;
    private Font sansFont;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnSceneLoaded()
    {
        // Dynamically instantiate the MainMenu manager at runtime so the user doesn't need to manually configure it in the editor
        GameObject menuObject = new GameObject("MainMenuController");
        menuObject.AddComponent<MainMenu>();
    }

    void Awake()
    {
        // If the player has already clicked Play in this session (e.g., they died and the scene reloaded), skip the menu
        if (hasPlayedOnce)
        {
            isMenuOpen = false;
            Time.timeScale = 1f;
            Destroy(gameObject);
            return;
        }
        else
        {
            isMenuOpen = true;
            Time.timeScale = 0f; // Pause the game on start
        }
    }

    void Start()
    {
        // Load the custom gradient background and times new roman font
        backgroundTexture = Resources.Load<Texture2D>("MainMenuBackground");
        serifFont = Resources.Load<Font>("Fonts/IAmMusic");
        if (serifFont == null)
        {
            serifFont = Font.CreateDynamicFontFromOSFont("Times New Roman", 120);
        }
        sansFont = Font.CreateDynamicFontFromOSFont("Arial", 65);
    }

    private bool DrawGlitchedButton(Rect rect, string text)
    {
        bool isHovering = rect.Contains(Event.current.mousePosition);
        Color textBaseColor = isHovering ? new Color(0.95f, 0.98f, 1f, 1f) : new Color(0.7f, 0.78f, 0.88f, 0.85f);

        Matrix4x4 originalMatrix = GUI.matrix;
        Vector2 btnPivot = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);

        // 1. Draw background serif text (Tall & Thin)
        GUIUtility.ScaleAroundPivot(new Vector2(0.6f, 1.6f), btnPivot);
        GUIStyle bgStyle = new GUIStyle();
        bgStyle.alignment = TextAnchor.MiddleCenter;
        bgStyle.font = serifFont;
        bgStyle.fontSize = 42;
        bgStyle.normal.textColor = new Color(textBaseColor.r, textBaseColor.g, textBaseColor.b, textBaseColor.a * 0.8f);
        GUI.Label(rect, text, bgStyle);

        // Restore matrix
        GUI.matrix = originalMatrix;

        // 2. Draw foreground sans-serif text (Wide & Shorter)
        GUIUtility.ScaleAroundPivot(new Vector2(1.05f, 0.8f), btnPivot);
        GUIStyle fgStyle = new GUIStyle();
        fgStyle.alignment = TextAnchor.MiddleCenter;
        fgStyle.font = sansFont;
        fgStyle.fontSize = 24;
        fgStyle.fontStyle = FontStyle.Bold;
        fgStyle.normal.textColor = new Color(textBaseColor.r * 0.9f, textBaseColor.g * 0.95f, textBaseColor.b * 1f, textBaseColor.a * 0.95f);
        GUI.Label(rect, text, fgStyle);

        // Restore matrix
        GUI.matrix = originalMatrix;

        // 3. Invisible button overlay to capture interaction
        return GUI.Button(rect, "", GUIStyle.none);
    }

    void OnGUI()
    {
        if (isMenuOpen)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            // Draw background image
            if (backgroundTexture != null)
            {
                GUI.DrawTexture(rect, backgroundTexture);
            }
            else
            {
                // Fallback dark teal background if texture is not loaded yet
                Color originalGUIColor = GUI.color;
                GUI.color = new Color(0.03f, 0.2f, 0.18f, 1f);
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = originalGUIColor;
            }

            if (currentState == MenuState.Home)
            {
                DrawHomeScreen();
            }
            else if (currentState == MenuState.HowToPlay)
            {
                DrawHowToPlayScreen();
            }
        }
    }

    private void DrawHomeScreen()
    {
        float itemHeight = 50f;
        float itemWidth = 300f;
        float centerX = (Screen.width - itemWidth) / 2f;
        float centerY = Screen.height / 2f;

        // Draw PLAY Button (Glitched Style) - Spaced at -130f
        Rect playRect = new Rect(centerX, centerY - 130f, itemWidth, itemHeight);
        if (DrawGlitchedButton(playRect, "PLAY"))
        {
            StartGame();
        }

        // Draw HOW TO PLAY Button (Glitched Style) - Spaced at -25f
        Rect howToPlayRect = new Rect(centerX, centerY - 25f, itemWidth, itemHeight);
        if (DrawGlitchedButton(howToPlayRect, "RULES"))
        {
            currentState = MenuState.HowToPlay;
        }

        // Draw EXIT Button (Glitched Style) - Spaced at +80f
        Rect exitRect = new Rect(centerX, centerY + 80f, itemWidth, itemHeight);
        if (DrawGlitchedButton(exitRect, "EXIT"))
        {
            ExitGame();
        }
    }

    private void DrawHowToPlayScreen()
    {
        float centerY = Screen.height / 2f;

        // 1. Draw Heading "RULES" in glitched serif style
        Rect titleRect = new Rect((Screen.width - 500f) / 2f, centerY - 220f, 500f, 80f);
        
        Matrix4x4 originalMatrix = GUI.matrix;
        Vector2 titlePivot = new Vector2(titleRect.x + titleRect.width / 2f, titleRect.y + titleRect.height / 2f);
        
        // Render heading tall and thin
        GUIUtility.ScaleAroundPivot(new Vector2(0.6f, 2.0f), titlePivot);
        GUIStyle titleBgStyle = new GUIStyle();
        titleBgStyle.alignment = TextAnchor.MiddleCenter;
        titleBgStyle.font = serifFont;
        titleBgStyle.fontSize = 65;
        titleBgStyle.normal.textColor = new Color(0.95f, 0.98f, 1f, 0.9f);
        GUI.Label(titleRect, "RULES", titleBgStyle);
        GUI.matrix = originalMatrix;

        // 2. Draw Rules Text
        GUIStyle rulesStyle = new GUIStyle();
        rulesStyle.font = sansFont;
        rulesStyle.fontSize = 24;
        rulesStyle.alignment = TextAnchor.MiddleCenter;
        rulesStyle.normal.textColor = new Color(0.72f, 0.8f, 0.9f, 0.85f);
        
        string rulesText = 
            "CONTROLS: USE WASD OR ARROW KEYS TO MOVE\n\n" +
            "OBJECTIVE: NAVIGATE TO THE FINISH ZONE\n\n" +
            "HAZARDS: AVOID FALLING BLOCKS AND SPINNING SPINNERS\n\n" +
            "RULES: ONE HIT AND YOU DIE. WATCH YOUR STEP!";
            
        Rect rulesRect = new Rect((Screen.width - 800f) / 2f, centerY - 70f, 800f, 250f);
        GUI.Label(rulesRect, rulesText, rulesStyle);

        // 3. Draw BACK Button (Glitched Style) at the bottom
        Rect backRect = new Rect((Screen.width - 300f) / 2f, Screen.height - 130f, 300f, 50f);
        if (DrawGlitchedButton(backRect, "BACK"))
        {
            currentState = MenuState.Home;
        }
    }

    private void StartGame()
    {
        isMenuOpen = false;
        hasPlayedOnce = true;
        Time.timeScale = 1f; // Resume game physics and time
        Destroy(gameObject); // Cleanup menu manager
    }

    private void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
