using System.Collections;
using UnityEngine;

public enum WeaponType
{
    Knife,
    Pistol,
    Shotgun,
    AR,
    AlmightyPush
}
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private float attackCooldown;
    private float nextAttackTime;

    protected Player player;
    [SerializeField] protected float recoilForce;

    [SerializeField] private int maxAmmo;
    private int curAmmo;

    private const float ANIMATION_DURATION = 0.5f;
    [SerializeField] private float reloadDuration;
    [SerializeField] private Animator reloadAnimator;
    private Coroutine reloadCoroutine = null;

    private AudioSource audioSource;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip noAmmoClip;
    

    private void Start()
    {
        curAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // stop reload coroutine and animation, otherwise it will break cuz the coroutine will never end
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }


        reloadAnimator.Play("Idle");
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    protected void KillIfEnemy(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<Enemy>().DieByWeapon(weaponType);
            GameManager.Instance.IncrementKillCount();
        }
    }

    public void Attack()
    {

        if (reloadCoroutine != null) return;

        if (Time.time < nextAttackTime) return;

        if (curAmmo == 0)
        {
            audioSource.PlayOneShot(noAmmoClip);
            nextAttackTime = Time.time + attackCooldown;
            return;
        }

        SpecificAttackBehaviour();

        // handle infinite ammo thing
        if (maxAmmo != -1) curAmmo--;

        nextAttackTime = Time.time + attackCooldown;

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(attackClip);
    }

    public void Reload()
    {
        if (maxAmmo == -1) return;

        if (reloadCoroutine != null) return;

        if (curAmmo == maxAmmo) return;

        reloadCoroutine = StartCoroutine(ReloadCoroutine());
        reloadAnimator.speed = ANIMATION_DURATION / reloadDuration;
        reloadAnimator.Play("Reload");
    }

    private IEnumerator ReloadCoroutine()
    {

        yield return new WaitForSeconds(reloadDuration);

        curAmmo = maxAmmo;

        reloadCoroutine = null;

    }

    protected abstract void SpecificAttackBehaviour();
}
