using UnityEngine;
using System.Collections;

public class Gamewintrigger : MonoBehaviour
{
    private bool isGameWon = false;
    private Font serifFont;
    private Font sansFont;

    private void InitializeFonts()
    {
        if (serifFont == null)
        {
            // Load Times New Roman directly (guaranteed to be on Windows and has very sharp serifs)
            serifFont = Font.CreateDynamicFontFromOSFont("Times New Roman", 120);
        }
        if (sansFont == null)
        {
            sansFont = Font.CreateDynamicFontFromOSFont("Arial", 65);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGameWon = true;
            Move moveScript = other.GetComponent<Move>();
            if (moveScript != null)
            {
                moveScript.enabled = false;
            }
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void DrawSmudgedLabel(Rect rect, string text, GUIStyle style)
    {
        Color originalColor = style.normal.textColor;
        int steps = 14;
        float blurAmount = 18f;

        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float offset = (t * 2f - 1f) * (blurAmount / 2f);
            float dist = Mathf.Abs(offset) / (blurAmount / 2f);
            float alphaFactor = Mathf.Clamp01(1f - dist * dist);
            
            style.normal.textColor = new Color(originalColor.r, originalColor.g, originalColor.b, originalColor.a * alphaFactor * 0.12f);
            // Offset horizontally (rect.x) since the text is horizontal now
            GUI.Label(new Rect(rect.x + offset, rect.y, rect.width, rect.height), text, style);
        }

        style.normal.textColor = originalColor;
        GUI.Label(rect, text, style);
    }

    private bool DrawGlitchedButton(Rect rect, string text)
    {
        bool isHovering = rect.Contains(Event.current.mousePosition);
        Color textBaseColor = isHovering ? new Color(0.95f, 0.98f, 1f, 1f) : new Color(0.7f, 0.78f, 0.88f, 0.85f);

        Matrix4x4 originalMatrix = GUI.matrix;
        Vector2 btnPivot = new Vector2(rect.x + rect.width / 2f, rect.y + rect.height / 2f);

        // 1. Draw background serif text (Tall & Thin)
        GUIUtility.ScaleAroundPivot(new Vector2(0.65f, 1.9f), btnPivot);
        GUIStyle bgStyle = new GUIStyle();
        bgStyle.alignment = TextAnchor.MiddleCenter;
        bgStyle.font = serifFont;
        bgStyle.fontSize = 55;
        bgStyle.normal.textColor = new Color(textBaseColor.r, textBaseColor.g, textBaseColor.b, textBaseColor.a * 0.8f);
        GUI.Label(rect, text, bgStyle);

        // Restore matrix
        GUI.matrix = originalMatrix;

        // 2. Draw foreground sans-serif text (Wide & Shorter)
        GUIUtility.ScaleAroundPivot(new Vector2(1.1f, 0.85f), btnPivot);
        GUIStyle fgStyle = new GUIStyle();
        fgStyle.alignment = TextAnchor.MiddleCenter;
        fgStyle.font = sansFont;
        fgStyle.fontSize = 35;
        fgStyle.fontStyle = FontStyle.Bold;
        fgStyle.normal.textColor = new Color(textBaseColor.r * 0.9f, textBaseColor.g * 0.95f, textBaseColor.b * 1f, textBaseColor.a * 0.95f);
        GUI.Label(rect, text, fgStyle);

        // Restore matrix
        GUI.matrix = originalMatrix;

        // 3. Invisible button overlay to capture interaction
        return GUI.Button(rect, "", GUIStyle.none);
    }

    private void OnGUI()
    {
        if (isGameWon)
        {
            InitializeFonts();

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            // 1. Draw solid off-white/light gray background
            Color originalGUIColor = GUI.color;
            GUI.color = new Color(0.92f, 0.92f, 0.92f, 1f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = originalGUIColor;

            Vector2 pivot = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Matrix4x4 originalMatrix = GUI.matrix;

            // 2. Scale to make the text tall, thin, and edgy (horizontal layout, no rotation)
            GUIUtility.ScaleAroundPivot(new Vector2(0.58f, 2.15f), pivot);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.font = serifFont;
            style.fontSize = 120;
            style.fontStyle = FontStyle.Normal;
            style.normal.textColor = new Color(0.08f, 0.08f, 0.08f, 1f); // Solid charcoal/black

            // 3. Draw the horizontal text with high-fidelity horizontal smear
            DrawSmudgedLabel(rect, "YOU WIN", style);

            // Restore Matrix for drawing UI buttons
            GUI.matrix = originalMatrix;

            // 4. Draw EXIT Button (Glitched Style) at the bottom
            float btnWidth = 300f;
            float btnHeight = 80f;
            float btnX = (Screen.width - btnWidth) / 2f;
            float btnY = Screen.height - 130f; // Bottom center placement
            Rect btnRect = new Rect(btnX, btnY, btnWidth, btnHeight);

            if (DrawGlitchedButton(btnRect, "EXIT"))
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
            }
        }
    }
}

