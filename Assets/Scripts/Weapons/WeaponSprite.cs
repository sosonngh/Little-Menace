using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    protected SpriteRenderer sr;
    private FollowMouse mouseScript;

    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private float muzzleSpawnOffsetX;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        mouseScript = transform.GetComponentInParent<FollowMouse>();
    }
    
    public virtual void AttackAnimation()
    {
        Vector2 spawnPos = (Vector2)transform.position + (Vector2)transform.right * muzzleSpawnOffsetX;

        // im crine
        Instantiate(muzzleFlash, spawnPos, mouseScript.RotationWithOffset);
    }

    private void Update()
    {
        sr.flipY = (mouseScript.LookVector.x < 0);
    }
}
