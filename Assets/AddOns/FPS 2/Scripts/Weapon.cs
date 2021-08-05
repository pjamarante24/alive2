using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    private int currentAmmo;
    private int leftAmmo;
    private bool inHand = false;
    private bool isReloading = false;
    private float nextFire = 0f;

    [Serializable]
    public class Config
    {
        public bool autoReload = true;
        public int maxAmmoPerMagazine = 12;
        public int maxAmmoPerWeapon = 12 * 4;
        public bool startWithMaxAmmo = true;
        public bool infinityAmmo = false;
        public float range = 75f;
        public float fireRate = 15f;
        public float impactForce = 30f;
        public float damage = 35f;
        public bool isSniper = false;
    }

    public Config config = new Config();

    [Serializable]
    public class Component
    {
        public Animator animator;
        public AudioSource mainAudioSource;
        public AudioSource shootAudioSource;
        public Cinemachine.CinemachineVirtualCamera playerCamera;
        public Text currentAmmoText;
        public Text leftAmmoText;
        public Transform shootPoint;
        public ParticleSystem muzzleFlash;
        public LineRenderer bulletTrail;
        public ParticleSystem impactEffect;
        public Sprite icon;
        public GameObject scopeOverlay;
        public WeaponHolder weaponHolder;
    }

    public Component component = new Component();

    [Serializable]
    public class soundClips
    {
        public AudioClip shootSound;
        public AudioClip takeOutSound;
        public AudioClip holsterSound;
        public AudioClip reloadSoundOutOfAmmo;
        public AudioClip reloadSoundAmmoLeft;
        public AudioClip outOfAmmoSound;
        public AudioClip aimSound;
    }
    public soundClips SoundClips;

    private void Awake()
    {
        // Disable animator
        component.animator.enabled = false;

        if (!component.animator) component.animator = GetComponent<Animator>();
        if (!component.playerCamera) component.playerCamera = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        if (!component.currentAmmoText) component.currentAmmoText = GameObject.Find("CurrentAmmoText").GetComponent<Text>();
        if (!component.leftAmmoText) component.leftAmmoText = GameObject.Find("LeftAmmoText").GetComponent<Text>();
        if (!component.scopeOverlay) component.scopeOverlay = FindInActiveObjectByName("ScopeOverlay");
        if (!component.weaponHolder) component.weaponHolder = GameObject.Find("WeaponHolder").GetComponent<WeaponHolder>();

        if (config.startWithMaxAmmo) currentAmmo = config.maxAmmoPerMagazine;
        leftAmmo = config.maxAmmoPerWeapon;
    }

    private void Update()
    {
        if (!inHand || isReloading) return;

        if (currentAmmo <= 0 && leftAmmo > 0 && config.autoReload)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < config.maxAmmoPerMagazine)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / config.fireRate;
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
        if (currentAmmo <= 0)
        {
            component.mainAudioSource.clip = SoundClips.outOfAmmoSound;
            component.mainAudioSource.Play();
            return;
        }

        currentAmmo--;

        SpawnMuzzleFlash();

        component.shootAudioSource.clip = SoundClips.shootSound;
        component.shootAudioSource.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, config.range, ~LayerMask.GetMask("Weapons")))
        {
            float damage = config.damage;
            if (component.weaponHolder) damage *= component.weaponHolder.damageMultiplier;


            Target target = hit.transform.GetComponent<Target>();
            if (target != null) target.TakeDamage(damage);

            Zombie zombie = hit.transform.GetComponent<Zombie>();
            if (zombie != null) zombie.TakeDamage(damage);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * config.impactForce);
            }

            SpawnBulletTrail(hit.point);
            SpawnImpactEffect(hit);
        }
        else
        {
            SpawnBulletTrail(ray.origin + ray.direction * config.range);
        }

        UpdateUI();
    }

    private void SpawnMuzzleFlash()
    {
        GameObject muzzleFlashObject = Instantiate(component.muzzleFlash.gameObject, component.shootPoint.position, Quaternion.identity);
        muzzleFlashObject.transform.parent = component.shootPoint.transform;
        muzzleFlashObject.GetComponent<ParticleSystem>().Emit(1);

        Destroy(muzzleFlashObject, 1f);
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulleTrailEffect = Instantiate(component.bulletTrail.gameObject, component.shootPoint.position, Quaternion.identity);

        LineRenderer lineRenderer = bulleTrailEffect.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, component.shootPoint.position);
        lineRenderer.SetPosition(1, hitPoint);

        Destroy(bulleTrailEffect, 1f);
    }

    private void SpawnImpactEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(component.impactEffect.gameObject, hit.point, Quaternion.LookRotation(hit.normal));
        impact.GetComponent<ParticleSystem>().Play();
        Destroy(impact, 2f);
    }

    private IEnumerator Scope()
    {
        component.animator.SetBool("Scope", true);

        component.mainAudioSource.clip = SoundClips.aimSound;
        component.mainAudioSource.Play();

        if (config.isSniper)
        {
            yield return new WaitForSeconds(.15f);
            component.scopeOverlay.SetActive(true);
            HideWeaponMask();
            component.playerCamera.m_Lens.FieldOfView = 15;
        }
        else
        {
            component.playerCamera.m_Lens.FieldOfView = 50;
        }
    }

    private void UnScope()
    {
        component.animator.SetBool("Scope", false);

        if (config.isSniper)
        {
            ShowWeaponMask();
            component.scopeOverlay.SetActive(false);
        }
        component.playerCamera.m_Lens.FieldOfView = 60;
    }

    IEnumerator Reload()
    {
        if (leftAmmo <= 0 && !config.infinityAmmo)
        {
            component.mainAudioSource.clip = SoundClips.outOfAmmoSound;
            component.mainAudioSource.Play();
            yield break;
        };

        if (currentAmmo <= 0) component.mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
        else component.mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
        component.mainAudioSource.Play();

        isReloading = true;
        component.animator.SetTrigger("Reload");
        float reloadTime = 1.3f;
        float reloadTimeMultiplier = 1;
        if (component.weaponHolder)
        {
            component.animator.SetFloat("ReloadSpeed", component.weaponHolder.reloadTimeMultiplier);
            reloadTimeMultiplier = component.weaponHolder.reloadTimeMultiplier;
        }
        yield return new WaitForSeconds(reloadTime - ((reloadTime * reloadTimeMultiplier) - reloadTime));

        int newAmmo = 0;
        if (config.infinityAmmo)
        {
            newAmmo = config.maxAmmoPerMagazine;
        }
        else
        {
            int missingAmmoInMagazine = config.maxAmmoPerMagazine - currentAmmo;
            if (leftAmmo >= missingAmmoInMagazine)
            {
                newAmmo = missingAmmoInMagazine;
            }
            else
            {
                newAmmo = leftAmmo;
            }

            leftAmmo -= newAmmo;
        }

        currentAmmo += newAmmo;
        isReloading = false;
        UpdateUI();
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
        if (inHand)
        {
            component.currentAmmoText.text = currentAmmo.ToString();
            if (config.infinityAmmo) component.leftAmmoText.text = "∞";
            else component.leftAmmoText.text = leftAmmo.ToString();
        }
        else
        {
            component.currentAmmoText.text = "";
            component.leftAmmoText.text = "";
        }
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    private void ShowWeaponMask()
    {
        Camera.main.cullingMask |= 1 << LayerMask.NameToLayer("Weapons");
    }

    private void HideWeaponMask()
    {
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Weapons"));
    }
}
