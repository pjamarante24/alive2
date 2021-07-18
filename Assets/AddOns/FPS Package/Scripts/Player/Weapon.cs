using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public float damage = 35f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    public bool autoReload = true;
    private bool isReloading = false;
    public bool isSniper = false;
    public GameObject scopeOverlay;

    public Camera fpsCamera;
    public GameObject weaponCamera;
    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public LineRenderer bulletTrail;
    public GameObject impactEffect;
    public Text currentAmmoText;
    public Text maxAmmoText;
    public Text weaponNameText;
    public Image weaponImage;
    public string weaponName;
    public Sprite weaponIcon;
    public Animator animator;
    private float nextFire = 0f;
    public WeaponSwitch weaponSwitch;

    public AudioSource mainAudioSource;
    public AudioSource shootAudioSource;
    [System.Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip aimSound;
    }
    public soundClips SoundClips;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
        UpdateUI();
    }

    void Update()
    {
        GameObject weaponInHand = weaponSwitch.weaponInHand();
        if (weaponInHand == null || weaponInHand != gameObject) return;
        if (isReloading || weaponSwitch.isSwitching) return;

        if (currentAmmo <= 0 && autoReload)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Scope());
        }

        if (Input.GetButtonUp("Fire2"))
        {
            UnScope();
        }
    }

    private void Shoot()
    {
        muzzleFlash.Emit(1);

        currentAmmo--;

        shootAudioSource.clip = SoundClips.shootSound;
        shootAudioSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, ~LayerMask.GetMask("Weapons")))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null) target.TakeDamage(damage);

            Zombie zombie = hit.transform.GetComponent<Zombie>();
            if (zombie != null) zombie.TakeDamage(damage);

            if (hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * impactForce);

            SpawnBulletTrail(hit.point);

            GameObject weaponImpactEffect = target?.impactEffect != null ? target.impactEffect : impactEffect;

            GameObject impact = Instantiate(weaponImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.GetComponent<ParticleSystem>().Play();
            Destroy(impact, 2f);
        }
        else
        {
            SpawnBulletTrail(fpsCamera.transform.position + fpsCamera.transform.forward * range);
        }

        UpdateUI();
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulleTrailEffect = Instantiate(bulletTrail.gameObject, shootPoint.position, Quaternion.identity);

        LineRenderer lineRenderer = bulleTrailEffect.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        Destroy(bulleTrailEffect, 1f);
    }

    IEnumerator Scope()
    {
        animator.SetBool("Scoping", true);

        mainAudioSource.clip = SoundClips.aimSound;
        mainAudioSource.Play();

        if (isSniper)
        {
            yield return new WaitForSeconds(.15f);
            scopeOverlay.SetActive(true);
            weaponCamera.SetActive(false);
            fpsCamera.fieldOfView = 15;
        }
        else
        {
            fpsCamera.fieldOfView = 50;
        }
    }

    private void UnScope()
    {
        animator.SetBool("Scoping", false);

        mainAudioSource.clip = SoundClips.aimSound;
        mainAudioSource.Play();

        if (isSniper) scopeOverlay.SetActive(false);
        fpsCamera.fieldOfView = 60;
        weaponCamera.SetActive(true);
    }

    IEnumerator Reload()
    {
        if (currentAmmo <= 0) mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
        else mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;

        mainAudioSource.Play();

        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateUI();

    }

    public void UpdateUI()
    {
        GameObject weaponInHand = weaponSwitch.weaponInHand();
        if (weaponInHand == null || weaponInHand != gameObject) return;

        weaponNameText.text = weaponName;
        weaponImage.sprite = weaponIcon;
        currentAmmoText.text = currentAmmo.ToString();
        maxAmmoText.text = maxAmmo.ToString();
    }
}
