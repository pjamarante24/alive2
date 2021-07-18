using UnityEngine;
using UnityEngine.UI;

public class WeaponPick : MonoBehaviour
{
    public GameObject playerCamera;
    public WeaponSwitch weaponSwitch;
    public Text pickUpText;
    private bool showingPickupText = false;

    private void Start()
    {
        weaponSwitch = GetComponent<WeaponSwitch>();
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2))
        {
            if (hit.transform.CompareTag("Weapon"))
            {
                ShowPickUpText();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    HidePickUpText();
                    weaponSwitch.PickWeapon(hit.transform.gameObject);
                }
            }
            else
            {
                HidePickUpText();
            }

        }
        else if (showingPickupText)
        {
            HidePickUpText();
        }
    }

    private void ShowPickUpText()
    {
        showingPickupText = true;
        pickUpText.text = "Press [F] to pick up.";
    }

    private void HidePickUpText()
    {
        showingPickupText = false;
        pickUpText.text = "";
    }
}
