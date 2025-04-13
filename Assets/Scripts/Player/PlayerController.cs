using Core.Extensions;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

#region Healthbar
public partial class PlayerController
{
    public RectTransform healthBar;
    public RectTransform healthBarBG;
    void StartHitEvents()
    {
        Owner.BindHitEvent(UpdateHealthbar);
    }
    void EndHitEvents()
    {
        Owner.ReleaseHitEvent(UpdateHealthbar);
    }
    public void UpdateHealthbar(HitPacket packet, BaseUnit unit)
    {
        float health = unit.CurrentHealth;
        float maxHealth = unit.MaxHealth;
        if (health <= 0)
        {
            TextPopupManager.PopupText(Owner.CurrentPosition, "MASSIVE##SKILL##ISSUE".ReplaceLineBreaks("##"));
        }
        healthBar.localScale = new Vector3(health / maxHealth, healthBar.localScale.y);
    }
}
#endregion
#region Collection
public partial class PlayerController
{
    public float collectionRadius;
    public float collectionSpeed;
    public LayerMask collectionLayer;
    private Collider2D[] collectableList;
    private Collider2D[] DetectCollectables()
    {
        return Physics2D.OverlapCircleAll(transform.position, collectionRadius, collectionLayer);
    }
    private void CollectionLoop()
    {
        collectableList = DetectCollectables();
        if (collectableList != null)
        {
            for (int i = 0; i < collectableList.Length; i++)
            {
                Collectable moveCollectable = collectableList[i].GetComponent<Collectable>();
                if (moveCollectable.isMovingToPlayer == false)
                {
                    moveCollectable.moveToPlayer = transform.position - collectableList[i].transform.position;
                    moveCollectable.collectionSpeed = collectionSpeed;
                    moveCollectable.isMovingToPlayer = true;
                }
            }
        }
    }
}
#endregion
#region Movement
public partial class PlayerController
{
    [field: SerializeField] public float MaxMoveSpeed { get; private set; } = 6f;
    private Vector2 moveInput;
    [SerializeField] float moveAcceleration = 60f;
    [SerializeField] float moveFriction = 40f;
    private bool isFacingRight = true;
    [SerializeField] private Transform PlayerMovementFlipAnchor;
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
}
#endregion
#region Camera Zoom
public partial class PlayerController
{
    [SerializeField] CinemachineCamera playerCam;
    float minOrthographicSize;
    float maxOrthographicSize => minOrthographicSize * MaxCameraZoomOut;
    [Range(1f, 5f)]
    [SerializeField] float MaxCameraZoomOut = 5f;
    private void ModifyOrthographicSize(float multiplier)
    {
        float orthographicSize = playerCam.Lens.OrthographicSize;
        orthographicSize += orthographicSize * (multiplier * 1f);
        playerCam.Lens.OrthographicSize = orthographicSize.Clamp(minOrthographicSize, maxOrthographicSize);
    }
}
#endregion
public partial class PlayerController : MonoBehaviour
{
    [field: SerializeField] public BaseUnit Owner { get; private set; }
    //wtf did fumorin do here
    private Rigidbody2D rb;
    public Rigidbody2D RB => rb;
    private void Awake()
    {
        minOrthographicSize = playerCam.Lens.OrthographicSize;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartHitEvents();
    }
    private void OnDestroy()
    {
        EndHitEvents();
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
        CollectionLoop();
    }
}
