using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    [SerializeField] private AudioClip openDoor;
    [SerializeField] private AudioClip closeDoor;
    [SerializeField] private AudioClip lockedDoor;
    [SerializeField] private AudioSource audioSource;
    public static DoorAudio instance;
    private void Awake()
    {     
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoorOpen()
    {
        audioSource.PlayOneShot(openDoor);
    }
    public void DoorClose()
    {
        audioSource.PlayOneShot(closeDoor);
    }
    public void DoorLocked()
    {
        audioSource.PlayOneShot(lockedDoor);
    }
}
