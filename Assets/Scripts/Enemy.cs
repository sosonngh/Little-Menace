using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;

    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem gettingShotFx;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Die()
    {
        Vector2 directionFromPlayer = transform.position - player.transform.position;
        float angle = Mathf.Atan2(directionFromPlayer.y, directionFromPlayer.x) * Mathf.Rad2Deg;

        Instantiate(gettingShotFx, transform.position, Quaternion.Euler(transform.rotation.x - angle, 90, 0));

        Destroy(gameObject);
    }

    private void Update()
    {
        Vector2 moveDirection = (player.transform.position - transform.position).normalized;

        transform.position += (Vector3)(speed * Time.deltaTime * moveDirection);
    }


}
