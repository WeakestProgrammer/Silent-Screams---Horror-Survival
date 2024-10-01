using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerCabinTwo : MonoBehaviour,IInteractable
{
    private Animator animOne;
    public bool isDrawerOneOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        animOne = GetComponent<Animator>();
    }
    public void Interact()
    {
        PlayDrawerOneAnim();
    }
    public void PlayDrawerOneAnim()
    {
        if (!isDrawerOneOpen)
        {
            animOne.Play("xabinOpen", 0, 0.0f);
            isDrawerOneOpen = true;
        }
        else
        {
            animOne.Play("xabinXlose1", 0, 0.0f);
            isDrawerOneOpen = false;
        }
    }
}
