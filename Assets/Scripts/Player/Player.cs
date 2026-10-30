using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int health;

    [SerializeField] private float linearRecoilRecoveryForce;

    public bool IsSpinning { get; private set; }
    public bool IsActive { get; private set; }

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [SerializeField] PlayerSprite sprite;

    [SerializeField] private Transform weaponHolder;
    private Weapon weapon;
    private bool isAttacking;

    private Vector2 recoilVelocity;

    // almighty pushhsh
    [SerializeField] private AlmightyPush almightyPush;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        weapon = weaponHolder.GetChild(0).GetComponent<Weapon>();
        weapon.SetPlayer(this);

        IsActive = true;
    }

    // getting hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsActive) return;

        if (!collision.CompareTag("Enemy")) return;

        health--;

        GameManager.Instance.UpdateHealthUI(health);

        if (health <= 0)
        {
            Die();
            return;
        }

        ActivateAlmightyPush();
    }

    private void ActivateAlmightyPush()
    {
        almightyPush.Attack();
        // anim
        sprite.Cry(almightyPush.shakeDuration + 0.5f);
    }

    private void Die()
    {
        GameManager.Instance.GameOver();

        sprite.Cry(10f);
        IsActive = false;
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (!IsActive) return;

        // check if the weapon is unlocked
        if (GameManager.Instance.currentPhase.latestWeaponUnlockedIndex < weaponIndex) return;

        weapon.gameObject.SetActive(false);

        weapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.SetPlayer(this);

        weapon.gameObject.SetActive(true);

        GameManager.Instance.SetSelectedWeaponUI(weaponIndex);
    }

    // INPUT METHODS
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsActive)
        {
            moveDirection = Vector2.zero; 
            return;
        }

        moveDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsActive) return;

        if (context.started) isAttacking = true;
        if (context.canceled) isAttacking = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!IsActive) return;

        weapon.Reload();
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (!IsActive) return;

        switch (context.control.name)
        {
            case "1": SwitchWeapon(0); break;
            case "2": SwitchWeapon(1); break;
            case "3": SwitchWeapon(2); break;
            case "4": SwitchWeapon(3); break;
            default: break;
        }
    }
    //

    public void ApplyLinearRecoil(Vector2 direction, float force)
    {
        if (!IsActive) return;

        recoilVelocity += force * direction;
    }

    public void ApplyAngularRecoil(float force)
    {
        if (!IsActive) return;

        rb.AddTorque(force);
    }

    private void FixedUpdate()
    { 
        if (IsSpinning)
        {
            rb.linearVelocity = recoilVelocity;
            // handle sprite behavior
            sprite.SetIsMoving(false);
        }
        else
        {
            rb.linearVelocity = speed * moveDirection + recoilVelocity;
            // handle sprite behavior
            sprite.SetIsMoving(moveDirection.magnitude > 0);
        }

        recoilVelocity = Vector2.MoveTowards(recoilVelocity, Vector2.zero, linearRecoilRecoveryForce);

        if (Mathf.Abs(rb.angularVelocity) < 180f)
        {
            if (IsSpinning)
            {
                IsSpinning = false;
                rb.angularVelocity = 0f;
                // animation
                sprite.StandUp();
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

        }
        else
        {
            IsSpinning = true;
        }
    }

    private void Update()
    {
        if (!IsActive) return;

        if (isAttacking) weapon.Attack();

    }

}
