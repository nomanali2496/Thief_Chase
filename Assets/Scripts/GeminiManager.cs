using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class GeminiRequest
{
    public Content[] contents;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}

[System.Serializable]
public class GeminiResponse
{
    public Candidate[] candidates;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

public class GeminiManager : MonoBehaviour
{
    [Header("Gemini API Settings")]
    [Tooltip("Get an API key from Google AI Studio")]
    public string apiKey = "YOUR_API_KEY_HERE";
    
    [Tooltip("The UI Text element to display the generated police chatter")]
    public TextMeshProUGUI radioChatterText;

    private const string API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

    // Call this method to generate dynamic chatter based on game events
    public void RequestDynamicChatter(float playerSpeed, string location, int wantedLevel)
    {
        string prompt = $"You are a police dispatcher in a high-speed chase game. " +
                        $"The suspect is currently driving at {playerSpeed:F0} mph near {location}. " +
                        $"Their wanted level is {wantedLevel} out of 5. " +
                        $"Give a short, thrilling, and urgent 1-sentence police radio dispatch update.";
        
        StartCoroutine(SendGeminiRequest(prompt));
    }

    private IEnumerator SendGeminiRequest(string promptText)
    {
        GeminiRequest requestData = new GeminiRequest
        {
            contents = new Content[]
            {
                new Content
                {
                    parts = new Part[]
                    {
                        new Part { text = promptText }
                    }
                }
            }
        };

        string jsonBody = JsonUtility.ToJson(requestData);
        string requestUrl = $"{API_URL}?key={apiKey}";

        using (UnityWebRequest request = new UnityWebRequest(requestUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Gemini API Error: {request.error}\nResponse: {request.downloadHandler.text}");
            }
            else
            {
                try
                {
                    GeminiResponse response = JsonUtility.FromJson<GeminiResponse>(request.downloadHandler.text);
                    if (response != null && response.candidates != null && response.candidates.Length > 0)
                    {
                        string generatedText = response.candidates[0].content.parts[0].text;
                        Debug.Log($"Gemini Police Dispatch: {generatedText}");
                        
                        if (radioChatterText != null)
                        {
                            radioChatterText.text = generatedText;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error parsing Gemini response: {e.Message}");
                }
            }
        }
    }
}
