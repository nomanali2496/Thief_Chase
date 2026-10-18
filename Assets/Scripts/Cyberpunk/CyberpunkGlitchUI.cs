using UnityEngine;
using TMPro;
using System.Collections;

public class CyberpunkGlitchUI : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private string originalText;
    
    [Header("Glitch Settings")]
    public float glitchChance = 0.05f;
    public float glitchDuration = 0.1f;
    public Color neonColor = new Color(0f, 1f, 1f, 1f); // Holographic Cyan
    
    private Color originalColor;
    private string glitchChars = "!@#$%^&*()_+-=0123456789<>?[]{}";

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        if(textMesh != null)
        {
            originalText = textMesh.text;
            originalColor = textMesh.color;
            StartCoroutine(GlitchRoutine());
        }
    }

    IEnumerator GlitchRoutine()
    {
        while(true)
        {
            // Random chance to glitch the text
            if(Random.value < glitchChance)
            {
                textMesh.color = neonColor;
                string glitched = "";
                for(int i = 0; i < originalText.Length; i++)
                {
                    if(originalText[i] != ' ' && Random.value < 0.3f)
                        glitched += glitchChars[Random.Range(0, glitchChars.Length)];
                    else
                        glitched += originalText[i];
                }
                textMesh.text = glitched;
                
                yield return new WaitForSeconds(glitchDuration);
                
                // Restore original state
                textMesh.text = originalText;
                textMesh.color = originalColor;
            }
            
            yield return new WaitForSeconds(0.1f); // Check frequently
        }
    }
    
    public void SetText(string newText)
    {
        originalText = newText;
        if(textMesh != null) textMesh.text = newText;
    }
}
