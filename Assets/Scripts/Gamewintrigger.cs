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
            serifFont = Font.CreateDynamicFontFromOSFont(new string[] { "Georgia", "Times New Roman", "Times" }, 110);
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

    private void OnGUI()
    {
        if (isGameWon)
        {
            InitializeFonts();

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            // 1. Draw solid black background
            Color originalGUIColor = GUI.color;
            GUI.color = Color.black;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);

            // 2. Draw subtle horizontal scanlines
            GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.15f);
            for (int y = 0; y < Screen.height; y += 4)
            {
                GUI.DrawTexture(new Rect(0, y, Screen.width, 2), Texture2D.whiteTexture);
            }
            GUI.color = originalGUIColor;

            Vector2 pivot = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Matrix4x4 originalMatrix = GUI.matrix;

            // 3. Draw background Serif text (Tall & Thin)
            GUIUtility.ScaleAroundPivot(new Vector2(0.7f, 1.8f), pivot);

            GUIStyle bgStyle = new GUIStyle();
            bgStyle.alignment = TextAnchor.MiddleCenter;
            bgStyle.font = serifFont;
            bgStyle.fontSize = 110;
            bgStyle.fontStyle = FontStyle.Normal;
            bgStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.85f);

            // Simple shadow for the background text
            GUIStyle bgShadowStyle = new GUIStyle(bgStyle);
            bgShadowStyle.normal.textColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
            GUI.Label(new Rect(rect.x + 2, rect.y + 2, rect.width, rect.height), "YOU WIN", bgShadowStyle);
            GUI.Label(rect, "YOU WIN", bgStyle);

            // Restore Matrix for foreground text
            GUI.matrix = originalMatrix;

            // 4. Draw horizontal glitch line in the middle of the screen
            GUI.color = new Color(1f, 1f, 1f, 0.08f);
            GUI.DrawTexture(new Rect(0, Screen.height / 2f - 15, Screen.width, 30), Texture2D.whiteTexture);
            GUI.color = originalGUIColor;

            // 5. Draw foreground Sans-Serif text (Wide & Shorter, with chromatic aberration)
            GUIUtility.ScaleAroundPivot(new Vector2(1.25f, 0.85f), pivot);

            GUIStyle fgStyle = new GUIStyle();
            fgStyle.alignment = TextAnchor.MiddleCenter;
            fgStyle.font = sansFont;
            fgStyle.fontSize = 65;
            fgStyle.fontStyle = FontStyle.Bold;

            // Chromatic Aberration offset labels:
            // Red offset
            fgStyle.normal.textColor = new Color(0.9f, 0.1f, 0.1f, 0.6f);
            GUI.Label(new Rect(rect.x + 3, rect.y, rect.width, rect.height), "YOU WIN", fgStyle);

            // Cyan/Blue offset
            fgStyle.normal.textColor = new Color(0.1f, 0.9f, 0.9f, 0.6f);
            GUI.Label(new Rect(rect.x - 3, rect.y, rect.width, rect.height), "YOU WIN", fgStyle);

            // Main foreground white text
            fgStyle.normal.textColor = new Color(0.95f, 0.95f, 0.98f, 0.95f);
            GUI.Label(rect, "YOU WIN", fgStyle);

            // Restore original GUI matrix
            GUI.matrix = originalMatrix;

            // 6. Draw Exit Button at the bottom (without matrix distortion)
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fontSize = 20;
            buttonStyle.font = sansFont;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = new Color(0.9f, 0.9f, 0.9f);

            float btnWidth = 180f;
            float btnHeight = 50f;
            float btnX = (Screen.width - btnWidth) / 2f;
            float btnY = Screen.height / 2f + 120f;

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

