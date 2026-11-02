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

    private const float RELOAD_ANIMATION_DURATION = 0.5f;
    [SerializeField] private float reloadDuration;
    [SerializeField] private Animator reloadAnimator;
    private Coroutine reloadCoroutine = null;

    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip noAmmoClip;
    [SerializeField] float audioVolume;

    [SerializeField] private float damage;

    [SerializeField] private WeaponSprite sprite;

    private void Awake()
    {
        ResetAmmo();
    }

    private void OnEnable()
    {
        // stop reload coroutine and animation, otherwise it will break cuz the coroutine will never end
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        reloadAnimator.Play("Idle", 0, 0f);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    protected void DealDamageIfEnemy(Collider2D collider)
    {
        // deal damage to boss
        if (collider.TryGetComponent<EnemyBoss>(out var enemyBoss))
        {
            enemyBoss.TakeDamage(damage, weaponType);
            return;
        }

        // kill other enemies
        if (collider.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.DieByWeapon(weaponType);
            GameManager.Instance.IncrementKillCount();
        }
        
    }

    public void Attack()
    {

        if (reloadCoroutine != null) return;

        if (Time.time < nextAttackTime) return;

        if (curAmmo == 0)
        {
            GameManager.Instance.SpawnNoAmmoMessage();
            PlayTemporarySound(noAmmoClip);
            nextAttackTime = Time.time + attackCooldown;
            return;
        }

        SpecificAttackBehaviour();

        // handle infinite ammo thing
        if (maxAmmo != -1) curAmmo--;

        nextAttackTime = Time.time + attackCooldown;

        PlayTemporarySound(attackClip);

        if (weaponType != WeaponType.AlmightyPush)
        {
            sprite.AttackAnimation();
        }
        
    }

    private void PlayTemporarySound(AudioClip clip)
    {
        GameObject temp = new("TemporaryAudioSource");

        temp.transform.position = transform.position;

        AudioSource audioSource = temp.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.volume = Mathf.Clamp01(audioVolume);
        audioSource.Play();

        Destroy(temp, clip.length);
    }

    public void ResetAmmo()
    {
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        curAmmo = maxAmmo;
    }

    public void Reload()
    {

        if (maxAmmo == -1) return;

        if (reloadCoroutine != null) return;

        if (curAmmo == maxAmmo) return;

        reloadCoroutine = StartCoroutine(ReloadCoroutine());
        reloadAnimator.speed = RELOAD_ANIMATION_DURATION / reloadDuration;
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
