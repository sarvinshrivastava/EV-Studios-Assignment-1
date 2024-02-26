using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool canJump;

    private float firstTapTime = 0f;
    private const float doubleTapTime = 0.2f;
    private const float jumpDelay = 0.3f; // Ensure this is longer than doubleTapTime

    private BallManager ballManager; // Reference to the BallManager script

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballManager = FindObjectOfType<BallManager>(); // Find the BallManager script in the scene
    }

    private void Update()
    {
        HandleTouchInput();
        RestrictMovementToScreenWidth();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition) && canJump)
            {
                HandleJumpOrDisappear();
            }
        }
        else if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved) && canJump)
        {
            HandleMovement();
        }
        else if (Input.touchCount == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void HandleJumpOrDisappear()
    {
        if (firstTapTime == 0f)
        {
            firstTapTime = Time.time;
        }
        else if (Time.time - firstTapTime < doubleTapTime)
        {
            ballManager.DisappearAndReappear();
            firstTapTime = 0f;
        }
        else
        {
            firstTapTime = Time.time;
            StartCoroutine(JumpAfterDelay(jumpDelay));
        }
    }

    private void HandleMovement()
    {
        if (Input.GetTouch(0).position.x < Screen.width / 2)
        {
            StartCoroutine(MoveAfterDelay(doubleTapTime, -moveSpeed));
        }
        else
        {
            StartCoroutine(MoveAfterDelay(doubleTapTime, moveSpeed));
        }
    }

    private void RestrictMovementToScreenWidth()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);
        position.x = Mathf.Clamp(position.x, 0.1f, 0.9f);
        transform.position = Camera.main.ViewportToWorldPoint(position);
    }

    private IEnumerator JumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private IEnumerator MoveAfterDelay(float delay, float velocityX)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}