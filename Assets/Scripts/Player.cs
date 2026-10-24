using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float health;
    [SerializeField] private float almightyPushRadius;

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

    // getting hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        health--;

        if (health <= 0)
        {
            Die();
        }

        ActivateAlmightyPush();
    }

    private void ActivateAlmightyPush()
    {
        // i hate it but i think trying to use weapon kill feature would be too much effort
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, almightyPushRadius);

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy")) continue;

            hit.GetComponent<Enemy>().DieByWeapon(WeaponType.AlmightyPush);
            GameManager.Instance.IncrementKillCount();
        }

        GameManager.Instance.ShakeCamera(0.5f, 0.5f);
    }

    private void Die()
    {
        SceneManager.LoadScene(0);
    }

    private void SwitchWeapon(int weaponIndex)
    {
        weapon.gameObject.SetActive(false);

        weapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.SetPlayer(this);

        weapon.gameObject.SetActive(true);
    }

    // INPUT METHODS
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) isAttacking = true;
        if (context.canceled) isAttacking = false;
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        weapon.Reload();
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
    //

    public void ApplyLinearRecoil(Vector2 direction, float force)
    {
        recoilVelocity += force * direction;
    }

    public void ApplyAngularRecoil(float force)
    {
        rb.AddTorque(force);
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
