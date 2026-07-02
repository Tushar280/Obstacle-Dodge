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
            serifFont = Resources.Load<Font>("Fonts/IAmMusic");
            if (serifFont == null)
            {
                serifFont = Font.CreateDynamicFontFromOSFont(new string[] { "Georgia", "Times New Roman", "Times" }, 110);
            }
        }
        if (sansFont == null)
        {
            sansFont = Font.CreateDynamicFontFromOSFont(new string[] { "Arial", "Helvetica", "Verdana", "sans-serif" }, 65);
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

            // 2. Scale to make the text tall and thin (horizontal layout, no rotation)
            GUIUtility.ScaleAroundPivot(new Vector2(0.7f, 1.9f), pivot);

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.font = serifFont;
            style.fontSize = 110;
            style.fontStyle = FontStyle.Normal;
            style.normal.textColor = new Color(0.08f, 0.08f, 0.08f, 1f); // Solid charcoal/black

            // 3. Draw the horizontal text with high-fidelity horizontal smear
            DrawSmudgedLabel(rect, "YOU WIN", style);

            // Restore Matrix for drawing UI buttons
            GUI.matrix = originalMatrix;

            // 4. Draw Exit Button at the bottom (without matrix distortion)
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fontSize = 20;
            buttonStyle.font = sansFont;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = new Color(0.1f, 0.1f, 0.1f);
            buttonStyle.hover.textColor = new Color(0.3f, 0.3f, 0.3f);

            float btnWidth = 180f;
            float btnHeight = 50f;
            float btnX = (Screen.width - btnWidth) / 2f;
            float btnY = Screen.height - 100f; // Bottom center placement

            if (GUI.Button(new Rect(btnX, btnY, btnWidth, btnHeight), "EXIT GAME", buttonStyle))
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

