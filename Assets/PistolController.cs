using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PistolController : MonoBehaviour
{

    // projectile
    private float ProjectileTapFiringRate = 0.8f;        // minimum delay between shots fired when fire button is tapped quickly and repeatedly
    protected float m_LastFireTime = 0.0f;
    protected float m_OriginalProjectileSpawnDelay = 0.0f;

    // motion
    public Vector3 MotionPositionRecoil = new Vector3(0, 0, -0.035f);   // positional force applied to weapon upon firing
    public Vector3 MotionRotationRecoil = new Vector3(-10.0f, 0, 0);    // angular force applied to weapon upon firing
    public float MotionRotationRecoilDeadZone = 0.5f;   // 'blind spot' center region for angular z recoil
    public float MotionDryFireRecoil = -0.1f;           // multiplies recoil when the weapon is out of ammo
    public float MotionRecoilDelay = 0.0f;				// delay between fire button pressed and recoil

    public float m_NextAllowedFireTime = 0;

    private GameController gameManager;
    private Animation anim_pistol;

    private int pistolIdleAnims = 4;
    private int currentPistolAnim = 1;

    private bool waitWithAnim = false;

    private bool playedStartAnimation = false;

    public ParticleSystem bulletTrail;
    public Light bulletFlash;

    private AudioSource pistolSoundSource;

    public AudioClip pistolFire;
    public AudioClip SoundDryFire;

    public Camera aimCamera;
    public Camera playerCamera;

    public FirstPersonController player;

    private RaycastHit hit;

    public Transform bulletStart;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        pistolSoundSource = this.GetComponent<AudioSource>();
        anim_pistol = this.GetComponent<Animation>();
        m_NextAllowedFireTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.weaponState != 2)
        {
            return;
        }

        if (!playedStartAnimation)
        {
            pickUpPistolAnim();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnAttempt_Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(player.pistolBullets > 0)
            {
                anim_pistol.Play("StandardReload");

                if(player.pistolBullets >= player.maxPistolBullets)
                {
                    if(player.loadedBullets > 0)
                    {
                        player.pistolBullets += player.loadedBullets;
                    }
                    player.loadedBullets = player.maxPistolBullets;
                    player.pistolBullets -=8;
                }
                else
                {
                    if (player.loadedBullets > 0)
                    {
                        player.pistolBullets += player.loadedBullets;
                    }
                    player.loadedBullets = player.pistolBullets;
                    player.pistolBullets = 0;
                }
                gameManager.updateBulletsPistol();
            }
            
        }

        if (!waitWithAnim)
        {
            idlePistolAnim(currentPistolAnim);
            currentPistolAnim++;
            if (currentPistolAnim > pistolIdleAnims) currentPistolAnim = 1;
            waitWithAnim = true;
            StartCoroutine("WaitWithNewAnim", Random.Range(2, 10));
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            this.transform.localPosition = new Vector3(-0.121f, -0.173f, 0.418f);
            aimCamera.fieldOfView = 10;
            playerCamera.fieldOfView = 45;
            aimCamera.nearClipPlane = 0.01f;
            
        }
        else if (aimCamera.fieldOfView != 30)
        {
            this.transform.localPosition = new Vector3(0f, -0.173f, -0.58f);
            aimCamera.fieldOfView = 30;
            playerCamera.fieldOfView = 60;
            aimCamera.nearClipPlane = 0.1f;
        }
    }

    public IEnumerator WaitWithNewAnim(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        waitWithAnim = false;

    }

    public void pickUpPistolAnim()
    {
        anim_pistol.Play("Wield");
        waitWithAnim = true;
        StartCoroutine("WaitWithNewAnim", Random.Range(2, 10));
        playedStartAnimation = true;
    }

    private void idlePistolAnim(int animID)
    {
        anim_pistol.Play("Idle0" + animID);
    }

    private void Fire()
    {
        m_LastFireTime = Time.time;

        m_NextAllowedFireTime = (m_LastFireTime + ProjectileTapFiringRate);

        bulletTrail.Play();
        anim_pistol.Play("Fire");
        bulletFlash.enabled = true;
        pistolSoundSource.clip = pistolFire;
        pistolSoundSource.Play();
        player.loadedBullets--;
        gameManager.updateBulletsPistol();

        Ray ray = Camera.main.ViewportPointToRay(new Vector2(.5f, .5f));

        ray.origin = bulletStart.position;

        if (Physics.Raycast(ray, out hit, 10000))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                enemy.TakeDamage(40);
            }
        }
    }

    private void DryFire()
    {

        if (pistolSoundSource != null)
        {
            pistolSoundSource.pitch = Time.timeScale;
            pistolSoundSource.PlayOneShot(SoundDryFire);
        }

        //DisableFiring();

        m_LastFireTime = Time.time;

    }

    private bool OnAttempt_Fire()
    {

        // weapon can only be fired when firing rate allows it
        if (Time.time < m_NextAllowedFireTime)
        {
            return false;
        }

        if (player.loadedBullets == 0)
        {
            DryFire();
            return false;
        }

        Fire();

        return true;

    }
}
