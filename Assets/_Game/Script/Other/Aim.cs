using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aim : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D hitCollider;        // Collider cần bật/tắt
    [SerializeField] private float overlapRadius = 0.1f;     // Bán kính kiểm tra va chạm thủ công
    [SerializeField] private LayerMask handLayer;            // Layer chứa đối tượng tay

    private int MaxHP = 3;
    private bool isDead;

    /*private void Update()
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
            // Với Android, cần thêm ID của touch để kiểm tra đúng
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            Stab();
        }
#endif
    }*/

    public void Stab()
    {
        if (isDead)
            return;

        anim.Play(CacheString.TAG_Stab_Pencil);
    }

    #region Damage Receiver
    private void TakeDamge()
    {
        MaxHP--;
        if (MaxHP <= 0)
        {
            isDead = true;
            AudioManager.Ins.PlaySFX(AudioManager.Ins.dead);
            UIManager.Ins.mainCanvas.Hit();
            Debug.Log("Die");

            Invoke(nameof(Loose), 1f);
        }
        else
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.hurt);
            UIManager.Ins.mainCanvas.Hit();
            Debug.Log("Take Damge");
        }

        ParticlePool.Play(ParticleType.BloodEff, transform.position, Quaternion.identity);
    }

    private void Loose()
    {
        UIManager.Ins.TransitionUI<ChangeUICanvas, MainCanvas>(0.5f,
               () =>
               {
                   LevelManager.Ins.DespawnLevel();
                   UIManager.Ins.OpenUI<LooseCanvas>();
               });
    }
    #endregion

    #region Animation Event
    /// <summary>
    /// Gọi trong animation bằng Animation Event lúc cần bật collider.
    /// </summary>
    public void EnableCollider()
    {
        hitCollider.enabled = true;
        Physics2D.SyncTransforms(); // Đồng bộ collider mới bật

        // Kiểm tra va chạm thủ công ngay sau khi bật
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, overlapRadius, handLayer);
        foreach (var hit in hits)
        {
            Hand hand = Cache.GetHand(hit);
            if (hand != null)
            {
                //Debug.Log("Hit (manual check)");
                // Xử lý va chạm tại đây nếu cần
                CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
                Handheld.Vibrate();
                TakeDamge();
                return;
            }
        }

        foreach (var hit in hits)
        {
            Table table = Cache.GetTable(hit);
            if (table != null)
            {
                //Debug.Log("Hit Table");
                // Xử lý logic nếu cần
                UIManager.Ins.mainCanvas.UpdatePoint(1);
                ParticlePool.Play(ParticleType.MagicEff, transform.position, Quaternion.identity);
                return;
            }
        }
    }

    /// <summary>
    /// Gọi trong animation bằng Animation Event lúc cần tắt collider.
    /// </summary>
    public void DisableCollider()
    {
        hitCollider.enabled = false;
    }
    #endregion
}
