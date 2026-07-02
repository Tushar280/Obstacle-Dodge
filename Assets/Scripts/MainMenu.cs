using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static bool hasPlayedOnce = false;
    private bool isMenuOpen = true;

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

    void OnGUI()
    {
        if (isMenuOpen)
        {
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            // 1. Draw background image
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

            // 2. Setup button styles (flat text buttons as in the reference image)
            GUIStyle buttonStyle = new GUIStyle();
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.font = serifFont;
            buttonStyle.fontSize = 50;
            buttonStyle.fontStyle = FontStyle.Normal;
            
            // Match the metallic lavender/blue text in the reference image
            buttonStyle.normal.textColor = new Color(0.7f, 0.78f, 0.88f, 0.85f);
            buttonStyle.hover.textColor = new Color(0.95f, 0.98f, 1f, 1f); // Glow white on hover
            buttonStyle.active.textColor = Color.cyan;

            float itemHeight = 80f;
            float itemWidth = 300f;
            float centerX = (Screen.width - itemWidth) / 2f;
            float centerY = Screen.height / 2f;

            // Draw Play Button
            Rect playRect = new Rect(centerX, centerY - 60f, itemWidth, itemHeight);
            if (GUI.Button(playRect, "Play", buttonStyle))
            {
                StartGame();
            }

            // Draw Exit Button
            Rect exitRect = new Rect(centerX, centerY + 30f, itemWidth, itemHeight);
            if (GUI.Button(exitRect, "Exit", buttonStyle))
            {
                ExitGame();
            }
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
