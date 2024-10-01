using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGenerate : MonoBehaviour
{
    [SerializeField] AudioClip dropSound;
    [SerializeField] AudioSource Source;
    [SerializeField] bool oneTime;
    // Start is called before the first frame update
    void Start()
    {
        oneTime = true;
        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name== "Cube (6)"&&oneTime) {
            Source.PlayOneShot(dropSound);
           
            Debug.Log("Xolliding");
            oneTime = false;
        }
    }

}
