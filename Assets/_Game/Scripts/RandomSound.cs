using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{

    public AudioClip[] clips;

    private void Start()
    {
        D.SoundManager.Instance.PlayCustomSfx(clips[Random.Range(0, clips.Length)]);
    }
}
