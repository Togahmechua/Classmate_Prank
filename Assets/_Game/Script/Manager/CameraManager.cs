using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private ViewportHandler cam;

    private void Start()
    {
        float aspectRatio = (float)Screen.height / Screen.width; // Vì đang làm màn hình dọc

        if (aspectRatio > 1.8f)
        {
            // iPhone (màn hình dài, thường > 18:9)
            cam.UnitsSize = 5.625f;
        }
        else
        {
            // iPad (gần vuông, 4:3)
            cam.UnitsSize = 8.6f;
        }
    }
}
