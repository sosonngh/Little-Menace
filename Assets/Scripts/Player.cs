using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float linearRecoilRecoveryForce;
    public bool IsSpinning { get; private set; }

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [SerializeField] private Transform weaponHolder;
    private Weapon weapon;
    private bool isAttacking;

    private Vector2 recoilVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = weaponHolder.GetChild(0).GetComponent<Weapon>();
    }

    private void SwitchWeapon(int weaponIndex)
    {
        weapon.gameObject.SetActive(false);

        weapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.SetPlayer(this);

        weapon.gameObject.SetActive(true);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) isAttacking = true;
        if (context.canceled) isAttacking = false;
    }

    public void ApplyLinearRecoil(Vector2 direction, float force)
    {
        recoilVelocity += force * direction;
    }

    public void ApplyAngularRecoil(float force)
    {
        rb.AddTorque(force);
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        switch (context.control.name)
        {
            case "1": SwitchWeapon(0); break;
            case "2": SwitchWeapon(1); break;
            case "3": SwitchWeapon(2); break;
            case "4": SwitchWeapon(3); break;
            default: break;
        }
    }

    private void FixedUpdate()
    {
        if (IsSpinning)
        {
            rb.linearVelocity = recoilVelocity;
        }
        else
        {
            rb.linearVelocity = speed * moveDirection + recoilVelocity;
        }

        recoilVelocity = Vector2.MoveTowards(recoilVelocity, Vector2.zero, linearRecoilRecoveryForce);

        if (Mathf.Abs(rb.angularVelocity) < 180f)
        {
            IsSpinning = false;
            rb.angularVelocity = 0f;
        }
        else
        {
            IsSpinning = true;
        }
    }

    private void Update()
    {
        if (isAttacking) weapon.Attack();
    }

}
