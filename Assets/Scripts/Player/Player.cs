using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int maxHealth;
    private int curHealth;

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

    private AudioSource audioSource;
    [SerializeField] AudioClip gameOverClip;

    [SerializeField] KnifeProp knifePropPrefab;

    // almighty pushhsh
    [SerializeField] private AlmightyPush almightyPush;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // getting hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsActive) return;

        if (!collision.CompareTag("Enemy")) return;

        curHealth--;

        GameManager.Instance.UpdateHealthUI(curHealth);

        if (curHealth <= 0)
        {
            StartCoroutine(Die());
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

    private void SwitchWeapon(int weaponIndex)
    {
        if (!IsActive) return;

        // check if the weapon is unlocked
        if (GameManager.Instance.currentPhase.latestWeaponUnlockedIndex < weaponIndex) return;

        DeselectWeapon();

        weapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.SetPlayer(this);
        weapon.gameObject.SetActive(true);

        GameManager.Instance.SetSelectedWeaponUI(weaponIndex);
    }

    private void DeselectWeapon()
    {
        if (weapon != null)
        {
            weapon.gameObject.SetActive(false);
        }
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

    // game over sequence methods
    private IEnumerator Die()
    {
        yield return StartCoroutine(Explode());

        StartCoroutine(GameManager.Instance.NewGame());
    }

    private IEnumerator Explode()
    {
        rb.linearVelocity = Vector2.zero;
        sprite.SetIsMoving(false);

        audioSource.PlayOneShot(gameOverClip);
        IsActive = false;

        yield return new WaitForSeconds(3f); // ANIMATION HERE

        sprite.SetSrEnabled(false);

        Instantiate(knifePropPrefab, transform.position, Quaternion.identity);
    }

    public IEnumerator Respawn()
    {
        DeselectWeapon();

        Vector3 initialPos = transform.position;
        transform.position = transform.position + 15 * Vector3.left;

        yield return new WaitForSeconds(2f);

        sprite.SetSrEnabled(true);
        sprite.SetIsMoving(true);

        while (transform.position != initialPos)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                initialPos, 
                speed * Time.deltaTime);

            yield return null;
        }

        sprite.SetIsMoving(false);

        yield return new WaitForSeconds(2f);

        IsActive = true;
        SwitchWeapon(0);
        curHealth = maxHealth;
        GameManager.Instance.UpdateHealthUI(curHealth);

    }

    private void FixedUpdate()
    {
        if (!IsActive) return;

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
