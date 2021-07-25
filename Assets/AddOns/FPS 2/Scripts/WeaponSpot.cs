using UnityEngine;
using UnityEngine.UI;

public class WeaponSpot : MonoBehaviour
{
    public GameObject weaponPrefab;
    public GameObject weaponModel;
    public bool destroyModelOnPickUp = true;
    public bool isBuySpot = false;
    public int cost = 400;
    public Player player;

    private void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public GameObject PickUpWeapon(Transform hand)
    {
        GameObject weapon = weaponPrefab;

        if (isBuySpot)
            if (!player.SubtractScore(cost)) return null;

        if (!weapon.activeInHierarchy)
            weapon = (GameObject)Instantiate(weapon);

        weapon.transform.SetParent(hand);
        Weapon weaponScript = weapon.GetComponent<Weapon>();
        weaponScript.component.animator.enabled = true;

        if (destroyModelOnPickUp)
        {
            Destroy(weaponModel);
            this.enabled = false;
        }

        return weapon;
    }

    public bool CanPickUpWeapon()
    {
        if (isBuySpot) return player.score.currentScore >= cost;

        return true;
    }

    public void ShowPickUpMessage(Text text)
    {
        if (isBuySpot)
            text.text = "Press [F] to buy weapon.\nCost: " + cost;
        else
            text.text = "Press [F] to pick up weapon.";
    }
}
