using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private float aimWiggleOffset;
    private float lookAngle;

    [SerializeField] private Player player;

    public void SetAimWiggleOffset(float degrees)
    {
        aimWiggleOffset = Random.Range(-degrees / 2, degrees / 2);
    }

    private void Update()
    {
        // don't track mouse if is spinning
        if (player.IsSpinning) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 lookDirection = mousePos - transform.position;

        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, lookAngle + aimWiggleOffset);
        
    }
}
