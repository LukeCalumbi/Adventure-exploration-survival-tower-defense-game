using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFade : MonoBehaviour
{
    public float fadeTime = 1f;

    Image panel;

    static Timer fadeTimer;
    static bool fadeIn = false;

    void Start()
    {
        panel = GetComponent<Image>();
        fadeTimer = new Timer(fadeTime);
        fadeIn = false;
        fadeTimer.ForceEnd();

        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this.transform.parent.gameObject);
    }

    void Update()
    {
        fadeTimer.Update(Time.deltaTime);

        if (fadeTimer.Finished())
        {
            panel.color = fadeIn ? Color.black : new Color(0f, 0f, 0f, 0f);
            return;
        }

        if (fadeIn)
            panel.color = Color.black * fadeTimer.GetCompletionPercentage();

        else
            panel.color = Color.black * fadeTimer.GetRemainingTimePercentage();
    }

    public static bool IsFadeInComplete()
    {
        return fadeTimer.Finished() && fadeIn;
    }

    public static bool IsFadeOutComplete()
    {
        return fadeTimer.Finished() && !fadeIn;
    }

    public static void StartFadeIn()
    {
        fadeIn = true;
        fadeTimer.Start();
    }

    public static void StartFadeOut()
    {
        fadeIn = false;
        fadeTimer.Start();
    }

    public static void StartNextFade()
    {
        fadeIn = !fadeIn;
        fadeTimer.Start();
    }
}
