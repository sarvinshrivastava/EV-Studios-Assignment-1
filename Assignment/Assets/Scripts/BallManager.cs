using UnityEngine;
using System.Collections;
using TMPro; // Import the TextMeshPro namespace

public class BallManager : MonoBehaviour {
    [SerializeField] GameObject ball;
    [SerializeField] float disappearanceDuration = 2f;
    [SerializeField] TextMeshProUGUI timeText; // Reference to the TextMeshProUGUI element

    public void DisappearAndReappear() {
        StartCoroutine(DisappearAndReappearCoroutine());
    }

    private IEnumerator DisappearAndReappearCoroutine() {
        ball.SetActive(false);
        float remainingTime = disappearanceDuration;
        while (remainingTime > 0) {
            timeText.text = "Time remaining: " + remainingTime; // Update the TextMeshProUGUI element
            yield return new WaitForSeconds(1f);
            remainingTime--;
        }
        ball.SetActive(true);
        timeText.text = ""; // Clear the TextMeshProUGUI element
    }
}