using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    [SerializeField] private Transform weaponHolder;
    private Weapon weapon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = weaponHolder.GetChild(0).GetComponent<Weapon>();
    }

    private void SwitchWeapon(int weaponIndex)
    {
        weapon.gameObject.SetActive(false);
        weapon = weaponHolder.GetChild(weaponIndex).GetComponent<Weapon>();
        weapon.gameObject.SetActive(true);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        weapon.Attack();
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
        rb.linearVelocity = speed * moveDirection;
    }

}
