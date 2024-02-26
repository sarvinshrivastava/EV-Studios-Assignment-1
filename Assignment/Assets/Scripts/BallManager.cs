using System.Collections;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private float disappearanceDuration = 2f;
    [SerializeField] private TextMeshProUGUI timeText; 

    // Starts the DisappearAndReappearCoroutine
    public void DisappearAndReappear()
    {
        StartCoroutine(DisappearAndReappearCoroutine());
    }

    // Coroutine to handle the disappearance and reappearance of the ball
    private IEnumerator DisappearAndReappearCoroutine()
    {
        ball.SetActive(false);
        float remainingTime = disappearanceDuration;

        while (remainingTime > 0)
        {
            timeText.text = "Time remaining: " + remainingTime; 
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }

        ball.SetActive(true);
        timeText.text = "";
    }
}