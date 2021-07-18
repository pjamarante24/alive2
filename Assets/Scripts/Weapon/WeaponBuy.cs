using UnityEngine;
using UnityEngine.UI;

public class WeaponBuy : MonoBehaviour
{
    public GameObject playerCamera;
    public WeaponSwitch weaponSwitch;
    public GameObject weapon;
    public PlayerScore playerScore;
    public int cost;
    public Text pickUpText;
    private bool showingPickupText = false;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 2))
        {
            if (hit.transform.CompareTag("WeaponBuy"))
            {
                ShowPickUpText();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (playerScore.SubtractScore(cost))
                    {
                        HidePickUpText();
                        weaponSwitch.PickWeapon(weapon);
                    }

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
        pickUpText.text = "Press [F] to buy weapon.\n" + "Cost: $" + cost;
    }

    private void HidePickUpText()
    {
        showingPickupText = false;
        pickUpText.text = "";
    }
}
