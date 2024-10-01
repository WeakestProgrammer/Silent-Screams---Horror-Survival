using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEfx : MonoBehaviour
{
    public bool start=false;
    public float duration;
    public AnimationCurve curve;
    public AudioSource audioSource;
    public AudioClip clip;
    public static ShakeEfx instance;
    bool oneTime;
    private void Awake()
    {
        instance = this;
        oneTime = false;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }
     IEnumerator Shaking()
    {
        Vector3 startPos= transform.position;
        float elapsedTime = 0f;
        while(elapsedTime<duration)
        {
            elapsedTime+= Time.deltaTime;
            float strength=curve.Evaluate(elapsedTime/duration);
            if(!oneTime||DeathScreen.Instance.twoTimes)
            {
                audioSource.PlayOneShot(clip);
                oneTime = true;
                DeathScreen.Instance.twoTimes = false;
            }            
            transform.position= startPos+Random.insideUnitSphere*strength;
            yield return null;
        }
        transform.position = startPos;
    }
}
