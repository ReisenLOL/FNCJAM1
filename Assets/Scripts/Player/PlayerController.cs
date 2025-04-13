using Core.Extensions;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //wtf did fumorin do here
    public float health;
    public float speed;
    private Rigidbody2D rb;
    public Rigidbody2D RB => rb;
    private Vector2 moveInput;
    [SerializeField] float moveAcceleration = 60f;
    [SerializeField] float moveFriction = 40f;
    [field: SerializeField] public float MaxMoveSpeed { get; private set; } = 6f;
    [SerializeField] CinemachineCamera playerCam;
    private bool isFacingRight = true;
    [SerializeField] private Transform PlayerMovementFlipAnchor;
    float minOrthographicSize;
    float maxOrthographicSize => minOrthographicSize * MaxCameraZoomOut;
    [Range(1f, 5f)]
    [SerializeField] float MaxCameraZoomOut = 5f;
    private void Awake()
    {
        minOrthographicSize = playerCam.Lens.OrthographicSize;
        rb = GetComponent<Rigidbody2D>();
    }
    private void ModifyOrthographicSize(float multiplier)
    {
        float orthographicSize = playerCam.Lens.OrthographicSize;
        orthographicSize += orthographicSize * (multiplier * 1f);
        playerCam.Lens.OrthographicSize = orthographicSize.Clamp(minOrthographicSize, maxOrthographicSize);
    }
    private bool TryMove(out Vector2 output)
    {
        output = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (output != Vector2.zero)
        {
            RB.VelocityTowards(moveInput.ScaleToMagnitude(MaxMoveSpeed), moveAcceleration);
            return true;
        }
        else
        {
            RB.VelocityTowards(Vector2.zero, moveFriction);
        }
        return false;
    }
    private bool ApplyMovementToSprite(Vector2 moveInput)
    {
        if (PlayerMovementFlipAnchor == null)
            return false;

        if (moveInput.x > 0f && !this.isFacingRight) //Right
        {
            Vector3 scale = PlayerMovementFlipAnchor.transform.localScale;
            PlayerMovementFlipAnchor.localScale = new(scale.x.Absolute(), scale.y, scale.z);
            isFacingRight = true;
            return true;
        }
        else if (moveInput.x < 0f && this.isFacingRight) //Left
        {
            Vector3 scale = PlayerMovementFlipAnchor.transform.localScale;
            PlayerMovementFlipAnchor.localScale = new(-(scale.x.Absolute()), scale.y, scale.z);
            isFacingRight = false;
            return true;
        }
        return false;
    }
    private void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        ModifyOrthographicSize(-scroll * 0.1f);
        if (TryMove(out moveInput))
        {
            if (ApplyMovementToSprite(moveInput))
            {

            }
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("MASSIVE SKILL ISSUE");
        }
    }
}
