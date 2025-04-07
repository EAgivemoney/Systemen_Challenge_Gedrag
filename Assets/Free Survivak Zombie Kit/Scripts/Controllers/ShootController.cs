using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShootController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;

    [Header("Reload Settings")]
    public int bulletsPerClip = 12;
    public int maxNumberOfClips = 5;
    private int numberOfClips;
    private int bulletsLeft;

    [Header("Weapon UI Settings")]
    public GameObject WeaponUI;
    public Text bulletNumberUI;
    public Text clipNumberUI;

    [Header("Weapon Objects")]
    public Transform gunEnd;
    public Animator animator;
    public GameObject FPSCamera;
    public Camera fpsCam;
    public PlayerController playerController;
    public AudioSource gunAudio;

    // Privates
    private WaitForSeconds shotDuration = new WaitForSeconds(0.05f);
    private float nextFire;

    private void OnEnable()
    {
        WeaponUI.SetActive(true);
    }

    private void OnDisable()
    {
        WeaponUI.SetActive(false);
    }

    private void Start()
    {
        // Initialize bullet system
        numberOfClips = maxNumberOfClips;
        bulletsLeft = bulletsPerClip;
        UpdateWeaponUI();
    }

    void Update()
    {
        ManageBulletCounts();

        if (playerController.inventory.activeSelf) return;

        if (playerController.currentMotion == PlayerController.MotionState.Idle)
        {
            HandleReload();
            HandleShooting();
        }
    }

    private void ManageBulletCounts()
    {
        bulletsLeft = Mathf.Clamp(bulletsLeft, 0, bulletsPerClip);
        numberOfClips = Mathf.Clamp(numberOfClips, 0, maxNumberOfClips);
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        if (WeaponUI.activeSelf)
        {
            bulletNumberUI.text = $"Bullets {bulletsLeft}/{bulletsPerClip}";
            clipNumberUI.text = $"Clips {numberOfClips}/{maxNumberOfClips}";
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < bulletsPerClip)
        {
            if (numberOfClips > 0)
            {
                Debug.Log("Reloading weapon");
                bulletsLeft = bulletsPerClip;
                numberOfClips--;
            }
            else
            {
                Debug.Log("No ammo to reload weapon");
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && IsAnimatorReadyToFire())
        {
            if (bulletsLeft > 0)
            {
                FireWeapon();
            }
            else
            {
                Debug.Log("No ammo in weapon");
            }
        }
        else
        {
            animator.SetBool("fire", false);
        }
    }

    private bool IsAnimatorReadyToFire()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        nextFire = Time.time + fireRate;

        StartCoroutine(ShotEffect());

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out RaycastHit hit, weaponRange))
        {
            EnemyController enemy = hit.collider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Damage(gunDamage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio?.Play();
        animator.SetBool("fire", true);
        FPSCamera.GetComponent<Crosshair>().IncreaseSpread(10);

        yield return shotDuration;
    }
}
