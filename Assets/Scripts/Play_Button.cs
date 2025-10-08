using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        // Get the Button component and add a listener
        Button playButton = GetComponent<Button>();
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void OnPlayButtonClicked()
    {
        // Load your game scene (replace "GameScene" with your actual scene name)
        SceneManager.LoadScene("Level 1");
        
        // Or load by build index:
        // SceneManager.LoadScene(1);
    }
}