using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private CameraAnchor[] camAnchor;
    [SerializeField] private Canvas cv;
    [SerializeField] private Aim aim;

    private void Start()
    {
        cv.renderMode = RenderMode.ScreenSpaceCamera;
        cv.worldCamera = Camera.main;

        UIManager.Ins.mainCanvas.ResetUI();

        StartCoroutine(IETurnOff());
    }

    private IEnumerator IETurnOff()
    {
        yield return new WaitForSeconds(1f);
        foreach (var cam in camAnchor)
        {
            cam.enabled = false;
        }
    }

    public void Stab()
    {
        aim.Stab();
    }
}