using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallMovement : MonoBehaviour {
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool canJump;

    private float firstTapTime = 0f;
    private const float doubleTapTime = 0.2f;
    private const float jumpDelay = 0.3f; // Make sure this is longer than doubleTapTime

    private BallManager ballManager; // Reference to the BallManager script

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ballManager = FindObjectOfType<BallManager>(); // Find the BallManager script in the scene
    }

    void Update() {
        // Check if the screen is tapped
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            // Convert the touch position to world space
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Check if the touch position overlaps with the ball's collider
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition) && canJump) {
                if (firstTapTime == 0f) {
                    firstTapTime = Time.time;
                } else if (Time.time - firstTapTime < doubleTapTime) {
                    ballManager.DisappearAndReappear(); // Call the DisappearAndReappear method in the BallManager script
                    firstTapTime = 0f;
                } else {
                    firstTapTime = Time.time;
                    StartCoroutine(JumpAfterDelay(jumpDelay)); // Add the jump functionality back
                }
            }
        }
        // Move left or right
        else if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved) && canJump) {
            if (Input.GetTouch(0).position.x < Screen.width / 2) {
                Debug.Log("Move left");
                StartCoroutine(MoveAfterDelay(doubleTapTime, -moveSpeed));
            } else {
                Debug.Log("Move right");
                StartCoroutine(MoveAfterDelay(doubleTapTime, moveSpeed));
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

    IEnumerator JumpAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    IEnumerator MoveAfterDelay(float delay, float velocityX) {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}