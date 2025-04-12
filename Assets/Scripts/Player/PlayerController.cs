using Core.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public GameObject projectile;
    private Camera playerCam;
    private bool isFacingRight = true;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerCamera;
    void Start()
    {
        playerCamera = GameObject.Find("Camera");
        playerSpriteRenderer = base.GetComponent<SpriteRenderer>();
        playerCam = GameObject.Find("Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();
        if (playerCam.orthographicSize > 1f || playerCam.orthographicSize > 0f && Input.mouseScrollDelta.y < 0)
        {
            playerCam.orthographicSize -= Input.mouseScrollDelta.y / 2;
        }
        else if (playerCam.orthographicSize < 0f)
        {
            playerCam.orthographicSize = 5f;
        }
        if (this.moveInput.x > 0f && this.isFacingRight)
        {
            playerSpriteRenderer.flipX = true;
            this.isFacingRight = !this.isFacingRight;
        }
        else if (this.moveInput.x < 0f && !this.isFacingRight)
        {
            playerSpriteRenderer.flipX = false;
            this.isFacingRight = !this.isFacingRight;
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }
}
