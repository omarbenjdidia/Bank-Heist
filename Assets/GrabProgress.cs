using UnityEngine;
using UnityEngine.UI;

public class GrabProgress : MonoBehaviour
{
    public Image progressBar; // Reference to the progress bar image
    public int totalGrabs = 3; // Total number of times the object can be grabbed
    private int currentGrabs = 0; // Current number of times the object has been grabbed


    public void Update()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        currentGrabs++;

        // Calculate progress as a percentage of total grabs
        float progress = (float)currentGrabs / (float)totalGrabs;

        // Clamp the progress value between 0 and 1
        progress = Mathf.Clamp01(progress);

        // Update the progress bar fill amount based on which section the progress falls into
        if (progress <= 0.33f)
        {
            progressBar.fillAmount = progress / 0.33f;
        }
        else if (progress <= 0.67f)
        {
            progressBar.fillAmount = (progress - 0.33f) / 0.34f;
        }
        else
        {
            progressBar.fillAmount = (progress - 0.67f) / 0.33f;
        }
    }
}
