using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playerAudio;

    public List<AudioClip> swordBlockingSounds = new List<AudioClip>();
    public List<AudioClip> swordHitBody = new List<AudioClip>();
    public List<AudioClip> kickHitBody = new List<AudioClip>();

    public List<AudioClip> femaleVoice1AttackSounds = new List<AudioClip>();
    public List<AudioClip> femaleVoice1DeathSounds = new List<AudioClip>();
    public List<AudioClip> femaleVoice1HurtSounds = new List<AudioClip>();
    
    public List<AudioClip> maleVoice1DeathSounds = new List<AudioClip>();
    public List<AudioClip> maleVoice1HurtSounds = new List<AudioClip>();

    public AudioSource objectiveAudio;
    public AudioClip objectiveHint;

    public AudioSource soundEffects;

    public AudioClip ReloadGun;
    public AudioClip pickupGun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playPickupGunSound()
    {
        soundEffects.PlayOneShot(pickupGun);
    }

    public void playReloadGunSound()
    {
        soundEffects.PlayOneShot(ReloadGun);
    }

    public void playObjectiveHintSound()
    {
        objectiveAudio.PlayOneShot(objectiveHint);
    }

    public void PlayHitSound()
    {
        playerAudio.PlayOneShot(swordHitBody[Random.Range(0, (swordHitBody.Count - 1))]);
    }

    public void PlayAttackSound_Female1()
    {
        playerAudio.PlayOneShot(femaleVoice1AttackSounds[Random.Range(0, (femaleVoice1AttackSounds.Count - 1))]);
    }

    public void PlayDeathSound_Male1()
    {
        playerAudio.PlayOneShot(maleVoice1DeathSounds[Random.Range(0, (maleVoice1DeathSounds.Count - 1))]);
    }

    public void PlayHurtSound_Male1()
    {
        playerAudio.PlayOneShot(maleVoice1HurtSounds[Random.Range(0, (maleVoice1HurtSounds.Count - 1))]);
    }

    public void PlayAttackSound_Female2(AudioSource enemySource)
    {
        enemySource.PlayOneShot(femaleVoice1AttackSounds[Random.Range(0, (femaleVoice1AttackSounds.Count - 1))]);
    }

    public void PlayDeathSound_Female2(AudioSource enemySource)
    {
        enemySource.PlayOneShot(femaleVoice1DeathSounds[Random.Range(0, (femaleVoice1DeathSounds.Count - 1))]);
    }

    public void PlayHurtSound_Female2(AudioSource enemySource)
    {
        enemySource.PlayOneShot(femaleVoice1HurtSounds[Random.Range(0, (femaleVoice1HurtSounds.Count - 1))]);
    }
}
