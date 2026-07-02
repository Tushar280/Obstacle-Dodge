using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHit : MonoBehaviour
{
    private int hitCount = 0;
    private Rigidbody rb;
    private Move moveScript;
    private bool isGameOver = false;
    private Font serifFont;
    private Font sansFont;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<Move>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player has been hit!");
        //Debug.Log(collision.gameObject.name + " hit the player");
        hitCount++;
        Debug.Log("Total hits = " + hitCount + " on player");

        if(collision.gameObject.CompareTag("Obstacle"))
        {
            MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            Material material = meshRenderer.material;
            material.color = Color.red;

            GameOver();
        }
    }

    private bool GameOver(){
        if (isGameOver) return false;
        isGameOver = true;
        // Apply physical game over state (make player fall and stop movement)
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
        }
        
        if (moveScript != null)
        {
            moveScript.enabled = false;
        }
        // Start the respawn sequence after a short delay
        StartCoroutine(RespawnSequence());
        return true;
    }
    private IEnumerator RespawnSequence()
    {
        // Wait for 2 seconds to allow the player to see the "Game Over" message
        yield return new WaitForSeconds(2f);
        // Reload the current scene to reset the player and all obstacle timers/states
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
        if (isGameOver)
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
            DrawSmudgedLabel(rect, "YOU DIED", style);

            // Restore original GUI matrix
            GUI.matrix = originalMatrix;
        }
    }
}
