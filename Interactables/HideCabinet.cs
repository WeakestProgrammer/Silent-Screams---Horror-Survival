using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCabinet : MonoBehaviour,IInteractable
{
    private Animator Hidecabin;
    public bool doorOpen=false;
    // Start is called before the first frame update
    void Start()
    {
        Hidecabin = GetComponent<Animator>();
    }
    public void Interact()
    {
        PlayAnimationDoors();
    }


   public void PlayAnimationDoors()
    {
        if (!doorOpen)
        {
            Hidecabin.Play("Hide", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            Hidecabin.Play("HideOut", 0, 0.0f);
            doorOpen=false;
        }
    }
}
