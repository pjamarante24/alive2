using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeapon : MonoBehaviour
{
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public Sprite primaryWeaponImage;
    public Sprite secondaryWeaponImage;
    GameObject currentWeapon;
    Sprite currentWeaponImage;

    private void Start()
    {
        currentWeapon = primaryWeapon;
        currentWeapon.SetActive(true);
    }

    public void changeWeapon(Image weaponImage)
    {
        currentWeapon.SetActive(false);

        if (currentWeapon == primaryWeapon)
        {
            currentWeapon = secondaryWeapon;
            changeWeaponImage(weaponImage, secondaryWeaponImage);
            currentWeapon.SetActive(true);
        }
        else
        {
            currentWeapon = primaryWeapon;
            changeWeaponImage(weaponImage, primaryWeaponImage);
            currentWeapon.SetActive(true);
        }
    }

    private void changeWeaponImage(Image weaponImage, Sprite newWeaponIcon)
    {
        weaponImage.sprite = newWeaponIcon;
    }
}
