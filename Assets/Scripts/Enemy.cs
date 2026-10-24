using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;

    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem gunshotBloodFx;
    [SerializeField] private ParticleSystem deathBloodFx;

    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DieByWeapon(WeaponType weaponType)
    {
        Vector2 directionFromPlayer = transform.position - player.transform.position;
        float angle = Mathf.Atan2(directionFromPlayer.y, directionFromPlayer.x) * Mathf.Rad2Deg;

        Instantiate(deathBloodFx, transform.position, Quaternion.Euler(0, 180, 0));

        if (weaponType is WeaponType.Pistol or WeaponType.AR or WeaponType.Shotgun)
        {
            Instantiate(gunshotBloodFx, transform.position, Quaternion.Euler(transform.rotation.x - angle, 90, 0));
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 moveDirection = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = speed * moveDirection;
    }


}
