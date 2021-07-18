using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip zombieMoan;
    public AudioClip zombieAttack1;
    public AudioClip zombieAttack2;
    public ZombieFollow zombieFollow;

    void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!zombieFollow) zombieFollow = GetComponent<ZombieFollow>();
    }

    void Update()
    {
        if (!audioSource) return;
        if (zombieFollow.isDead){ 
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }
        if (!audioSource.isPlaying) PlayZombieMoan();
    }

    public void PlayZombieMoan()
    {
        audioSource.clip = zombieMoan;
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.PlayDelayed(Random.Range(0.5f, 2.5f));
    }

    public void PlayZombieAttack()
    {
        AudioClip clip;
        if (Random.Range(0, 2) == 0) clip = zombieAttack1;
        else clip = zombieAttack2;
        audioSource.clip = clip;
        audioSource.pitch = Random.Range(0.5f, 1.5f);
        audioSource.Play();
    }
}
