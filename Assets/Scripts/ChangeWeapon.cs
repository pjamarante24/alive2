using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;

    GameObject currentWeapon;

    private void Start()
    {
        currentWeapon = primaryWeapon;
        currentWeapon.SetActive(true);
    }

    public void changeWeapon()
    {
        Debug.Log("Current Weapon: " + currentWeapon);

        currentWeapon.SetActive(false);

        if (currentWeapon == primaryWeapon)
        {
            currentWeapon = secondaryWeapon;
            currentWeapon.SetActive(true);
        }
        else
        {
            currentWeapon = primaryWeapon;
            currentWeapon.SetActive(true);
        }

        Debug.Log("New Weapon: " + currentWeapon);
    }
}
