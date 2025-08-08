using EasyTextEffects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button stabBtn;
    [SerializeField] private Button pauseBtn;

    [SerializeField] private TextMeshProUGUI pointTxt;
    [SerializeField] private TextEffect pointEff;

    [SerializeField] private Animator anim;

    [SerializeField] private GameObject bloodyScreen;

    private Coroutine bloodyCoroutine;
    private int startPoint;

    private void Start()
    {
        UIManager.Ins.mainCanvas = this;

        pauseBtn.onClick.AddListener(() =>
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });

        stabBtn.onClick.AddListener(() =>
        {
            if (LevelManager.Ins.curLevel == null)
                return;

            LevelManager.Ins.curLevel.Stab();
        });
    }

    #region Point
    public void UpdatePoint(int point)
    {
        anim.Play(CacheString.TAG_PopUp_MainCanvas);
        startPoint += point;
        if (startPoint <= 9)
        {
            pointTxt.text = "0" + startPoint.ToString();

        }
        else
        {
            pointTxt.text = startPoint.ToString();
        }

        pointEff.Refresh();
    }

    #endregion

    #region Bloody Screen

    public void Hit()
    {
        if (bloodyCoroutine != null)
        {
            StopCoroutine(bloodyCoroutine);
            bloodyCoroutine = null;
        }

        bloodyCoroutine = StartCoroutine(BloodyScreenEffect());
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (!bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(true);

        var image = bloodyScreen.GetComponentInChildren<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float t = 0f;

        while (t < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            t += Time.deltaTime;
            yield return null;
        }

        if (bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(false);

        bloodyCoroutine = null;
    }

    public void ResetUI()
    {
        startPoint = 0;
        pointTxt.text = "0" + startPoint.ToString();


        if (bloodyCoroutine != null)
        {
            StopCoroutine(bloodyCoroutine);
            bloodyCoroutine = null;
        }

        if (bloodyScreen.activeInHierarchy)
            bloodyScreen.SetActive(false);
    }

    #endregion
}
