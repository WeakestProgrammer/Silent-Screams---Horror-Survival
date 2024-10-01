using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour,IInteractable
{
    private Animator drawerAnim;
    public bool drawerOpen=false;
    public static Drawer instance;
    void Start()
    {
        drawerAnim = GetComponent<Animator>();
    }
    private void Awake()
    {
        instance = this;
    }
    public void Interact()
    {
       PlayAnimationDrawer();
        Debug.Log("running");
    }
    public void PlayAnimationDrawer()
    {
        if(!drawerOpen)
        {
            drawerAnim.Play("openBookshelf", 0, 0.0f);
            drawerOpen = true;
        }
        else
        {
            drawerAnim.Play("xloseBookShelf", 0, 0.0f);
            drawerOpen = false;
        }
    }
}
