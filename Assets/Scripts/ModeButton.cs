using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModeButton : MonoBehaviour
{
    public int modeID = 0;           // 0 = Car Mode, 1 = Bike, 2 = Heli, 3 = Highway Chase
    public GameObject lockIcon;
    public Image backgroundImage;

    [Header("Colors")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("UnlockedMode_" + modeID, modeID == 0 ? 1 : 0);

        if (unlocked == 1)
            UnlockMode();
        else
            LockMode();
    }

    public void UnlockMode()
    {
        if (lockIcon != null) lockIcon.SetActive(false);
        if (button != null) button.interactable = true;
        if (backgroundImage != null) backgroundImage.color = unlockedColor;
    }

    public void LockMode()
    {
        if (lockIcon != null) lockIcon.SetActive(true);
        if (button != null) button.interactable = false;
        if (backgroundImage != null) backgroundImage.color = lockedColor;
    }

    public void OnClick()
    {
        if (!button.interactable) return;
        Debug.Log("Mode " + modeID + " Selected");
        // Yahan game start logic aayega baad mein
    }
}