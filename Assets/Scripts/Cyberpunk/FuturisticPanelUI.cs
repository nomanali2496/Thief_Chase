using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FuturisticPanelUI : MonoBehaviour
{
    private Image panelImage;

    [Header("Futuristic Glass Settings")]
    public Color glassColor = new Color(0.02f, 0.05f, 0.1f, 0.75f);
    public Color glowingBorderColor = new Color(0f, 1f, 1f, 1f); // Cyan
    public float borderThickness = 2f;

    private Outline outline;

    void Awake()
    {
        panelImage = GetComponent<Image>();
        panelImage.color = glassColor;

        // Ensure it doesn't have a default white sprite that ruins the glass look
        if (panelImage.sprite != null && panelImage.sprite.name == "UISprite")
        {
            panelImage.sprite = null; 
        }

        // Add Glowing Outline
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        
        outline.effectColor = glowingBorderColor;
        outline.effectDistance = new Vector2(borderThickness, -borderThickness);
    }
    
    // Optional: Add a pulse effect to the border
    void Update()
    {
        if (outline != null)
        {
            float pulse = Mathf.PingPong(Time.time * 2f, 0.5f) + 0.5f;
            Color pulseColor = glowingBorderColor;
            pulseColor.a = pulse;
            outline.effectColor = pulseColor;
        }
    }
}
