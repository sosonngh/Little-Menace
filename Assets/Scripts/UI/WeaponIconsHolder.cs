using UnityEngine;

public class WeaponIconsHolder : MonoBehaviour
{
    [SerializeField] private WeaponIconUI[] weaponIcons;

    public void SetSelectedWeapon(int weaponIndex)
    {
        foreach (WeaponIconUI icon in weaponIcons)
        {
            icon.SetSelected(false);
        }

        weaponIcons[weaponIndex].SetSelected(true);
    }

    public void SetWeaponLocked(int weaponIndex, bool state)
    {
        weaponIcons[weaponIndex].SetLocked(state);
    }
}
