using D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float lifeTime;
    public AudioClip sfx;

    private void Start()
    {
        SoundManager.Instance.PlayCustomSfx(sfx);
        Destroy(gameObject, lifeTime);
    }
}
