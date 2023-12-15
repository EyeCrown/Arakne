using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class akPlaySound : MonoBehaviour
{

    public AK.Wwise.Event CurrentSound;
    public void PlaySound()
    {
        CurrentSound.Post(gameObject);
    }

    public void StopSound()
    {
        CurrentSound.Stop(gameObject);
    }
}

