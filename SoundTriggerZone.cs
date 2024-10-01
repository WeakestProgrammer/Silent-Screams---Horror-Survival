using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerZone : MonoBehaviour
{
    [Header("BG")]
    [SerializeField] AudioSource ambienceBg;
    [SerializeField] AudioSource insideBg;
    [SerializeField]bool isPlayingInsideBg;

    // Start is called before the first frame update
    void Start()
    {
        isPlayingInsideBg = false;
        ambienceBg.volume=1.0f;
        insideBg.volume=0f;
    }

    // Update is called once per frame
    void Update()
    {
        //checking Enviroment And changing Sound
        if (!isPlayingInsideBg)
        {
            ambienceBg.volume += Time.deltaTime * 0.20f;
            insideBg.volume -= Time.deltaTime * 0.20f;
        }
        else return;
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name== "Character")
        {          
            ambienceBg.volume-=Time.deltaTime*0.20f;
            insideBg.volume+=Time.deltaTime*0.20f;
            isPlayingInsideBg=true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Character")
        { 
            isPlayingInsideBg = false;
        }
    }

}
