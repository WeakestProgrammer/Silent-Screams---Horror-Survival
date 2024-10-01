using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isTrigger;
    [SerializeField] private GameObject door;
    [SerializeField] private Door targetDoor;
    public bool isDoorLocked;
    public static DoorTrigger instance;
    public TextMeshProUGUI DoorText;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isTrigger = false;
    
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTrigger)
        {
            if (other.gameObject.name == "Character"&&targetDoor.doorOpen)
            {
                isTrigger = true;
                door.GetComponent<Animator>().Play("CloseDoor", 0, 0.0f);
                DoorAudio.instance.DoorClose();
                door.GetComponent<Door>().enabled = false;
                StartCoroutine(waitforfinish());
                Debug.Log("if Running");
            }
            else if(other.gameObject.name == "Character"&&!targetDoor.doorOpen)
            {
                isTrigger = true;
                Debug.Log("Else Running");
                door.GetComponent<Door>().enabled = false;
                StartCoroutine(waitforfinish());
            }
        }
        
    }
    IEnumerator waitforfinish()
    {
        yield return new WaitForSeconds(1.5f);
        door.GetComponent<Animator>().enabled = false;
      
        isDoorLocked = true;
    }
    public void DoorTextVisible()
    {
        DoorText.text = "Door is Locked Now, Find Another Way";
        StartCoroutine(Waitfordisplay());
    }
    IEnumerator Waitfordisplay()
    {
        yield return new WaitForSeconds(2f);
        DoorText.gameObject.SetActive(false);
    }
}
