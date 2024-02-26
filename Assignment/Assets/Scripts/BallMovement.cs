using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallMovement : MonoBehaviour {
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float disappearanceDuration = 2f;

    [SerializeField] TextMeshProUGUI timerText;

    private Rigidbody2D rb;
    private bool canJump ;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Check if the ball is tapped
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            // Convert the touch position to world space
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Check if the touch position overlaps with the ball's collider
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition) && canJump) {
                Debug.Log("Jump");
                rb.velocity = new Vector2(0, jumpForce);
                canJump = false;
            }
        }

        // Move left or right
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)) {
            if (Input.GetTouch(0).position.x < Screen.width / 2) {
                Debug.Log("Move left");
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            } else {
                Debug.Log("Move right");
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }

        // Stop moving when there's no input
        else if (Input.touchCount == 0) {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Restrict movement to screen width
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x, 0.1f, 0.9f);
        transform.position = Camera.main.ViewportToWorldPoint(position);
    }

    IEnumerator DisappearAndReappear(){
        float timer = disappearanceDuration;

        while (timer > 0f) {
            timerText.text = timer.ToString("F1"); 
            yield return new WaitForSeconds(0.1f); 
            timer -= 0.1f;
        }

        timerText.text = "0.0"; 
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1f); 
        gameObject.SetActive(true);
        canJump = true; 
        timerText.text = "";
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    void OnMouseDown() {
        if (canJump) {
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }
    }
}