using System;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : MonoBehaviour
{
    private bool inHand = false;
    private float nextFire = 0f;

    [Serializable]
    public class Config
    {
        public float range = 3f;
        public float fireRate = 1f;
        public float impactForce = 10f;
        public float damage = 60f;
    }

    public Config config = new Config();

    [Serializable]
    public class Component
    {
        public Animator animator;
        public AudioSource mainAudioSource;
        public Cinemachine.CinemachineVirtualCamera playerCamera;
        public Text currentAmmoText;
        public Text leftAmmoText;
    }

    public Component component = new Component();

    [Serializable]
    public class soundClips
    {
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
    }
    public soundClips SoundClips;

    private void Awake()
    {
        if (!component.animator) component.animator = GetComponent<Animator>();
        if (!component.playerCamera) component.playerCamera = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (!component.currentAmmoText) component.currentAmmoText = GameObject.Find("CurrentAmmoText").GetComponent<Text>();
        if (!component.leftAmmoText) component.leftAmmoText = GameObject.Find("LeftAmmoText").GetComponent<Text>();
    }

    private void Update()
    {
        if (!inHand) return;


        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / config.fireRate;
            Attack();
        }
    }

    private void Attack()
    {
        component.animator.SetTrigger("Attack");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, config.range, ~LayerMask.GetMask("Weapons")))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null) target.TakeDamage(config.damage);

            Zombie zombie = hit.transform.GetComponent<Zombie>();
            if (zombie != null) zombie.TakeDamage(config.damage);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * config.impactForce);
            }
        }
    }

    public void TakeOutWeapon()
    {
        component.mainAudioSource.clip = SoundClips.takeOutSound;
        component.mainAudioSource.Play();

        inHand = true;
        component.animator.SetBool("Hold", false);
        UpdateUI();
    }

    public void HolsterWeapon()
    {
        component.mainAudioSource.clip = SoundClips.holsterSound;
        component.mainAudioSource.Play();

        inHand = false;
        component.animator.SetBool("Hold", true);
        UpdateUI();
    }

    public void UpdateUI()
    {
        component.currentAmmoText.text = "";
        component.leftAmmoText.text = "";
    }
}
