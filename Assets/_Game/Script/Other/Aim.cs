using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    void Update()
    {
#if UNITY_EDITOR || UNITY_IOS
        if (Input.GetMouseButtonDown(0))
        {
            Stab();
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Stab();
        }
#endif
    }

    private void Stab()
    {
        anim.Play(CacheString.TAG_Stab_Pencil);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Hand hand = Cache.GetHand(other);
        if (hand != null )
        {
            Debug.Log("Hit");
        }
    }
}
