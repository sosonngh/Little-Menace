using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private float aimWiggleOffset;
    private float lookAngle;

    [SerializeField] private Player player;

    // animaition purposes
    [SerializeField] PlayerSprite playerSprite;

    [SerializeField] Texture2D cursorTexture;

    public Quaternion RotationWithOffset { get; private set; }
    public Vector2 LookVector { get; private set; }


    private void Start()
    {
        // change the cursor texture and make it confined within screen
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetAimWiggleOffset(float degrees)
    {
        aimWiggleOffset = Random.Range(-degrees / 2, degrees / 2);
    }

    private void Update()
    {
        if (!player.IsActive) return;

        // don't track mouse if is spinning
        if (player.IsSpinning) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        LookVector = mousePos - transform.position;

        lookAngle = Mathf.Atan2(LookVector.y, LookVector.x) * Mathf.Rad2Deg;
        RotationWithOffset = Quaternion.Euler(0, 0, lookAngle + aimWiggleOffset);
        transform.rotation = RotationWithOffset;

        // animation
        playerSprite.SetFlip(LookVector.x < 0);
       
    }
}
