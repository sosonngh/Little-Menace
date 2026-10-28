using UnityEngine;

public class WeaponIconsHolder : MonoBehaviour
{
    private WeaponIconUI[] weaponIcons;

    private void Awake()
    {
        weaponIcons = GetComponentsInChildren<WeaponIconUI>();
    }

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
