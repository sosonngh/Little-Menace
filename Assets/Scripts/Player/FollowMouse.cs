using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private float aimWiggleOffset;

    private float drawOffset;
    [SerializeField] float drawSpeed;
    private Coroutine drawCoroutine;

    private float lookAngle;

    [SerializeField] private Player player;

    // animaition purposes
    [SerializeField] PlayerSprite playerSprite;

    [SerializeField] Texture2D cursorTexture;

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

    public Coroutine StartDrawWeapon()
    {
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);
            drawCoroutine = null;
        }

        drawCoroutine = StartCoroutine(DrawWeapon());
        return drawCoroutine;
    }

    private IEnumerator DrawWeapon()
    {
        drawOffset = -90f;

        while (drawOffset < 0)
        {
            drawOffset = Mathf.MoveTowards(drawOffset, 0, drawSpeed * Time.deltaTime);
            yield return null;
        }

        drawCoroutine = null;
    }

    private void Update()
    {
        if (!player.IsActive) return;

        // don't track mouse if is spinning
        if (player.IsSpinning) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        LookVector = mousePos - transform.position;

        lookAngle = Mathf.Atan2(LookVector.y, LookVector.x) * Mathf.Rad2Deg;
        float finalDrawOffset = LookVector.x < 0 ? -drawOffset : drawOffset;
        Quaternion rotationWithOffset = Quaternion.Euler(0, 0, lookAngle + aimWiggleOffset + finalDrawOffset);
        transform.rotation = rotationWithOffset;

        // animation
        playerSprite.SetFlip(LookVector.x < 0);
       
    }
}
