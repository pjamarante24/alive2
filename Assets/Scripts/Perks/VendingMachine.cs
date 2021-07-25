using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    public Text message;
    private Perks perks;
    public Perks.Type type;
    public AudioSource audioSource;

    [System.Serializable]
    public class SoundClips
    {
        public AudioClip vendingMachine;
        public AudioClip openAndDrinkCan;
    }
    public SoundClips soundClips;

    private void Start()
    {
        if (!perks) perks = GameObject.FindGameObjectWithTag("Player").GetComponent<Perks>();
    }

    public IEnumerator AddPerk()
    {
        audioSource.clip = soundClips.vendingMachine;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.clip = soundClips.openAndDrinkCan;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        perks.AddPerk(type);
    }

    public void ShowMessage()
    {
        message.text = "Press [F] to buy " + type.ToString() + " perk.";
    }

    public void HideMessage()
    {
        if (message.text == "") return;
        message.text = "";
    }
}
