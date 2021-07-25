using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Perks : MonoBehaviour
{
    public Player player;
    public StarterAssets.FirstPersonController playerController;
    public PlayerHealth playerHealth;
    public WeaponHolder weaponHolder;
    public VendingMachine lastVendingMachine;
    public enum Type
    {
        Speed,
        Reload,
        Health,
        Damage
    };

    public int cost = 2500;

    public Text messageText;

    private void Start()
    {
        if (!player) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!playerHealth) playerHealth = player.GetComponent<PlayerHealth>();
        if (!playerController) playerController = GameObject.Find("PlayerCapsule").GetComponent<StarterAssets.FirstPersonController>();
        if (!weaponHolder) weaponHolder = GameObject.Find("WeaponHolder").GetComponent<WeaponHolder>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.gameObject.tag == "Vending Machine")
            {
                VendingMachine vendingMachine = hit.collider.gameObject.GetComponent<VendingMachine>();
                vendingMachine.ShowMessage();

                if (Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(vendingMachine.AddPerk());
                }

                lastVendingMachine = vendingMachine;
            }
            else if (lastVendingMachine)
            {
                lastVendingMachine.HideMessage();
            }
        }
        else if (lastVendingMachine)
        {
            lastVendingMachine.HideMessage();
        }
    }

    public void AddPerk(Type perk)
    {
        if (player.SubtractScore(cost))
        {
            switch (perk)
            {
                case Type.Speed:
                    IncreaseSpeed();
                    break;

                case Type.Reload:
                    IncreaseReload();
                    break;

                case Type.Health:
                    IncreaseHealth();
                    break;

                case Type.Damage:
                    IncreaseDamage();
                    break;
            }

            StartCoroutine(showPerkAddedMessage(perk));
        }
    }

    private void IncreaseSpeed()
    {
        playerController.MoveSpeed = playerController.MoveSpeed * 1.1f;
        playerController.SprintSpeed = playerController.SprintSpeed * 1.1f;
    }

    private void IncreaseReload()
    {
        weaponHolder.reloadTimeMultiplier = weaponHolder.reloadTimeMultiplier * 1.1f;
    }

    private void IncreaseHealth()
    {
        playerHealth.maxHealth = playerHealth.maxHealth * 1.1f;
        playerHealth.currentHealth = playerHealth.maxHealth;
    }

    private void IncreaseDamage()
    {
        weaponHolder.damageMultiplier = weaponHolder.damageMultiplier * 1.1f;
    }

    private string getPerkMessage(Type perk)
    {
        switch (perk)
        {
            case Type.Speed:
                return "Speed++";
            case Type.Reload:
                return "Reload++";
            case Type.Health:
                return "Health++";
            case Type.Damage:
                return "Damage++";
        }

        return "";
    }

    private IEnumerator showPerkAddedMessage(Type perk)
    {
        messageText.text = getPerkMessage(perk);
        yield return new WaitForSeconds(2);
        messageText.text = "";
    }
}
