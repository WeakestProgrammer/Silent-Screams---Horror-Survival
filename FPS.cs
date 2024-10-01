using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
     public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;

    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;

    void Start()
    {
        fpsText=GetComponent<TextMeshProUGUI>();
        timeLeft = updateInterval;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0f)
        {
            float fps = accum / frames;
            fpsText.text = string.Format("FPS: {0:F2}", fps);

            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}
