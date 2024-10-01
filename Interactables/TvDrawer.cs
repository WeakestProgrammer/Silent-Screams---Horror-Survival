using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TvDrawer : MonoBehaviour,IInteractable
{
    private Animator tvDrawerAnim;
    public bool tvDrawerOpen=false;
    // Start is called before the first frame update
    void Start()
    {
        tvDrawerAnim = GetComponent<Animator>();
    }
    public void Interact()
    {
        PlayAnimationTvDrawer();
    }
    public void PlayAnimationTvDrawer()
    {
        if (!tvDrawerOpen)
        {
            tvDrawerAnim.Play("Tv", 0, 0.0f);
            tvDrawerOpen = true;
        }
        else
        {
            tvDrawerAnim.Play("tvXlose", 0, 0.0f);
            tvDrawerOpen = false;
        }
    }
}
