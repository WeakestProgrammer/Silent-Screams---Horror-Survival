using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    private Animator doorAnim;
    public bool doorOpen = false;
    public static Door instane;
    public bool isDoorLocked = false;
    public TextMeshProUGUI textDisplay;

    private void Start()
    {
        
        doorAnim = GetComponent<Animator>();      
    }
    private void Awake()
    {
        instane = this;
    }

    public void Interact()
    {
        if (GetComponent<Animator>().enabled)
        {         
            PlayAnimationDoor();      
        }
        else if (DoorTrigger.instance.isDoorLocked)
        {
            DoorAudio.instance.DoorLocked();
            DoorTrigger.instance.DoorText.gameObject.SetActive(true);
            DoorTrigger.instance.DoorTextVisible();
        }
        //Debug.Log("Priting");
    }

    public void PlayAnimationDoor()
    {
        if (!doorOpen&&!isDoorLocked)
        {
            doorAnim.Play("OpenDoor", 0, 0.0f);
            DoorAudio.instance.DoorOpen();
            doorOpen = true;
        }
        else if(!isDoorLocked)
        {
            doorAnim.Play("CloseDoor", 0, 0.0f);
            DoorAudio.instance.DoorClose();
            doorOpen = false;
        }

        if (isDoorLocked)
        {
            textDisplay.gameObject.SetActive(true);
            textDisplay.text = "Door is Locked";
            StartCoroutine(Waitfordisplay());
        }
    }
    IEnumerator Waitfordisplay()
    {
        yield return new WaitForSeconds(2f);
        textDisplay.gameObject.SetActive(false);
    }

}
