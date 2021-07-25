using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHolder : MonoBehaviour
{
    private int maxWeapons = 2;
    public List<GameObject> weapons = new List<GameObject>();
    public GameObject currentWeapon;
    public GameObject meleeWeapon;
    public Text centerMessageText;
    public Image primaryWeaponImage;
    public Image secondaryWeaponImage;
    public float damageMultiplier = 1f;
    public float reloadTimeMultiplier = 1f;

    private void Start()
    {
        currentWeapon = meleeWeapon;
        TakeOutWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(1);
        }

        // Raycast to see if there is a weapon in front of the player
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.gameObject.tag == "WeaponSpot")
            {
                WeaponSpot weaponSpot = hit.collider.gameObject.GetComponent<WeaponSpot>();
                if (weaponSpot && weaponSpot.enabled)
                {
                    weaponSpot.ShowPickUpMessage(centerMessageText);
                    if (Input.GetKeyDown(KeyCode.F) && weaponSpot.CanPickUpWeapon())
                    {
                        if (weapons.Count < maxWeapons)
                            AddWeapon(weaponSpot);
                        else
                            ReplaceWeapon(weaponSpot);
                    }
                }
            }
            else
            {
                centerMessageText.text = "";
            }
        }
        else
        {
            centerMessageText.text = "";
        }
    }

    public void AddWeapon(WeaponSpot weaponSpot)
    {
        GameObject weapon = weaponSpot.PickUpWeapon(transform);
        weapons.Add(weapon);
        ChangeWeapon(weapons.Count - 1);
        centerMessageText.text = "";
    }

    public void ReplaceWeapon(WeaponSpot weaponSpot)
    {
        GameObject weapon = weaponSpot.PickUpWeapon(transform);
        int index = weapons.IndexOf(currentWeapon);

        if (index != -1 && weapon)
        {
            weapons[index] = weapon;
            ChangeWeapon(index);
        }
    }

    public void ChangeWeapon(int index)
    {
        GameObject newWeapon = weapons.ElementAtOrDefault(index);
        if (currentWeapon != newWeapon)
        {
            HolsterWeapon();
            currentWeapon = newWeapon;
            TakeOutWeapon();
        }
        else
        {
            HolsterWeapon();
            currentWeapon = meleeWeapon;
            TakeOutWeapon();
        }

        UpdateUI();
    }

    private void HolsterWeapon()
    {
        if (currentWeapon)
        {
            Weapon weapon = currentWeapon.GetComponent<Weapon>();
            if (weapon) weapon.HolsterWeapon();

            MeleeWeapon meleeWeapon = currentWeapon.GetComponent<MeleeWeapon>();
            if (meleeWeapon) meleeWeapon.HolsterWeapon();
        }
    }

    private void TakeOutWeapon()
    {
        if (currentWeapon)
        {
            Weapon weapon = currentWeapon.GetComponent<Weapon>();
            if (weapon) weapon.TakeOutWeapon();

            MeleeWeapon meleeWeapon = currentWeapon.GetComponent<MeleeWeapon>();
            if (meleeWeapon) meleeWeapon.TakeOutWeapon();
        }
    }

    private void UpdateUI()
    {
        Weapon primaryWeapon = weapons?.ElementAtOrDefault(0)?.GetComponent<Weapon>();
        Weapon secondaryWeapon = weapons?.ElementAtOrDefault(1)?.GetComponent<Weapon>();

        Color activeColor = Color.white;
        activeColor.a = 200f / 255f;

        Color trasparentColor = Color.white;
        trasparentColor.a = 0 / 255f;

        Color color = Color.white;
        color.a = 163f / 255f;

        if (primaryWeapon)
        {
            if (primaryWeapon.component.icon)
            {
                primaryWeaponImage.sprite = primaryWeapon.component.icon;
                primaryWeaponImage.color = primaryWeapon.name == currentWeapon?.name ? activeColor : color;
            }
            else
            {
                primaryWeaponImage.sprite = null;
                primaryWeaponImage.color = trasparentColor;
            }
        }

        if (secondaryWeapon)
        {
            if (secondaryWeapon.component.icon)
            {
                secondaryWeaponImage.sprite = secondaryWeapon.component.icon;
                secondaryWeaponImage.color = secondaryWeapon.name == currentWeapon?.name ? activeColor : color;
            }
            else
            {
                secondaryWeaponImage.sprite = null;
                secondaryWeaponImage.color = trasparentColor;
            }
        }
    }
}
