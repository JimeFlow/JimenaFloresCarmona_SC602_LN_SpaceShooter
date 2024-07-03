using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // https://www.dafont.com/
    // FIND A FONT TO USE IT ON THE TEXTMESHPRO CONTROLS => 10 PUNTOS
    
    [SerializeField]
    TextMeshProUGUI scoreTextbox;

    [SerializeField]    
    Transform livesContainer;

    bool hasLives = true;

    private void Awake()
    {
        // IMPLEMENT Singleton (instance) TO INVOKE IncreaseScore => 10 PUNTOS
    }

    public void IncreaseScore(float points)
    {
        float score = float.Parse(scoreTextbox.text);
        score += points;
        scoreTextbox.text = score.ToString();
    }

    public void DecreaseLives()
    {
        int maxLiveNumber = 0;
        Image[] liveImages = livesContainer.GetComponentsInChildren<Image>();
        Image maxLiveImage = null;

        foreach (Image liveImage in liveImages)
        {
            if (liveImage.name.StartsWith("Live-") && liveImage.enabled)
            {
                int liveNumber = int.Parse(liveImage.name.Remove(0, 5));

                if (maxLiveNumber == 0 || liveNumber > maxLiveNumber)
                {
                    maxLiveNumber = liveNumber;
                    maxLiveImage = liveImage;
                }
            }
        }

        if (maxLiveImage != null)
        {
            maxLiveImage.enabled = false;
        }

        hasLives = maxLiveNumber > 0;
    }

    public bool HasLives()
    {
        return hasLives;
    }
}
