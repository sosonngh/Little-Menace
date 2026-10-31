using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;

    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem gunshotBloodFx;
    [SerializeField] private ParticleSystem deathBloodFx;
    [SerializeField] private ParticleSystem deathSubBloodFx;

    [SerializeField] private SpriteAnim spriteChild;

    private Rigidbody2D rb;
    private AudioSource audioSource;


    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void DieByWeapon(WeaponType weaponType)
    {
        Vector2 directionFromPlayer = transform.position - player.transform.position;
        float angle = Mathf.Atan2(directionFromPlayer.y, directionFromPlayer.x) * Mathf.Rad2Deg;

        Instantiate(deathBloodFx, transform.position, Quaternion.Euler(0, 180, 0));
        Instantiate(deathSubBloodFx, transform.position, Quaternion.Euler(0, 180, 0));

        if (weaponType is WeaponType.Pistol or WeaponType.AR or WeaponType.Shotgun)
        {
            Instantiate(gunshotBloodFx, transform.position, Quaternion.Euler(transform.rotation.x - angle, 90, 0));
        }

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position, 1.75f);

        // enemies died by almighty push don't spawn new ones
        if (TryGetComponent<SpawnEnemiesOnDefeat>(out var spawner) && weaponType != WeaponType.AlmightyPush)
        {
            spawner.Spawn();
        }

        Destroy(gameObject);

    }

    private void FixedUpdate()
    {
        Vector2 moveDirection = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = speed * moveDirection;

        spriteChild.SetIsMoving(moveDirection.magnitude > 0);
        spriteChild.SetFlip(moveDirection.x > 0);
    }


}
