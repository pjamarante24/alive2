using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSwitch : MonoBehaviour
{
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;
    public GameObject primaryWeaponHolder;
    public GameObject secondaryWeaponHolder;
    public string currentWeapon;
    public float switchTime = 1f;
    public Animator animator;
    public bool isSwitching = false;

    void Start()
    {
        currentWeapon = "Primary";
        if (primaryWeapon) primaryWeapon.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) StartCoroutine(ChangeWeapon());
    }

    IEnumerator ChangeWeapon()
    {
        isSwitching = true;
        animator.SetBool("Switching", true);
        yield return new WaitForSeconds(switchTime);
        if (currentWeapon == "Primary")
        {
            if (primaryWeapon) primaryWeapon.SetActive(false);
            if (secondaryWeapon) secondaryWeapon.SetActive(true);

            currentWeapon = "Secondary";

            if (secondaryWeapon)
            {
                Weapon weaponScript = secondaryWeapon.GetComponent<Weapon>();
                if (weaponScript != null) weaponScript.UpdateUI();
            }
        }
        else if (currentWeapon == "Secondary")
        {
            if (secondaryWeapon) secondaryWeapon.SetActive(false);
            if (primaryWeapon) primaryWeapon.SetActive(true);

            currentWeapon = "Primary";

            if (primaryWeapon)
            {
                Weapon weaponScript = primaryWeapon.GetComponent<Weapon>();
                if (weaponScript != null) weaponScript.UpdateUI();
            }
        }



        animator.SetBool("Switching", false);
        isSwitching = false;
    }

    public GameObject weaponInHand()
    {
        if (currentWeapon == "Primary") return primaryWeapon;

        return secondaryWeapon;
    }

    public void PickWeapon(GameObject weapon)
    {
        if (currentWeapon == "Primary")
        {
            if (primaryWeapon != null)
            {
                primaryWeapon.transform.SetParent(null);
                primaryWeapon.transform.position = weapon.transform.position;
                primaryWeapon.transform.rotation = weapon.transform.rotation;
            }

            primaryWeapon = weapon;
            weapon.transform.SetParent(primaryWeaponHolder.transform);
        }
        else if (currentWeapon == "Secondary")
        {
            if (secondaryWeapon != null)
            {
                secondaryWeapon.transform.SetParent(null);
                secondaryWeapon.transform.position = weapon.transform.position;
                secondaryWeapon.transform.rotation = weapon.transform.rotation;
            }

            secondaryWeapon = weapon;
            weapon.transform.SetParent(secondaryWeaponHolder.transform);
        }

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        Weapon weaponScript = weapon.GetComponent<Weapon>();
        if (weaponScript != null) weaponScript.UpdateUI();
    }
}
