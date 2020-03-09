using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        pistolSoundSource = this.GetComponent<AudioSource>();
        anim_pistol = this.GetComponent<Animation>();
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
            bulletTrail.Play();
            anim_pistol.Play("Fire");
            bulletFlash.enabled = true;
            pistolSoundSource.clip = pistolFire;
            pistolSoundSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim_pistol.Play("StandardReload");
        }

        if (!waitWithAnim)
        {
            idlePistolAnim(currentPistolAnim);
            currentPistolAnim++;
            if (currentPistolAnim > pistolIdleAnims) currentPistolAnim = 1;
            waitWithAnim = true;
            StartCoroutine("WaitWithNewAnim", Random.Range(2, 10));
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
}
