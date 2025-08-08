using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [SerializeField] private Button pauseBtn;

    private void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            //AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
            UIManager.Ins.OpenUI<PauseCanvas>();
            UIManager.Ins.CloseUI<MainCanvas>();
        });
    }
}
