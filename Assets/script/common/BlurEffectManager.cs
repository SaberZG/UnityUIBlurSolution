using System;
using UnityEngine;
public class BlurEffectManager : MonoBehaviour
{   
    private static BlurEffectManager _instance; 
    public static BlurEffectManager Instance{
        get{
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BlurEffectManager)) as BlurEffectManager;
            }
            return _instance;
        }
    }
    // 获取模糊脚本
    public ScreenBlurEffect main_blur_effect;
    public ScreenBlurEffect ui_blur_effect;
    void Awake()
    {
        if(main_blur_effect == null)
        {
            main_blur_effect = GameObject.Find("MainCamera").GetComponent<ScreenBlurEffect>();
        }
        if(ui_blur_effect == null)
        {
            ui_blur_effect = GameObject.Find("UICamera").GetComponent<ScreenBlurEffect>();
        }
    }

    // 提供模糊截屏
    public void EnableBlurScreenshot(bool use_ui_camera, BlurData data = null, Action<RenderTexture> callback = null)
    {
        if (use_ui_camera)
        {
            ui_blur_effect.EnableBlurRender(BlurType.ScreenShot, data, callback);
        }
        else
        {
            main_blur_effect.EnableBlurRender(BlurType.ScreenShot, data, callback);
        }
    }

    // 提供摄像机模糊
    public void EnableBlurCameraEffect(bool use_ui_camera, BlurData data = null)
    {
        if (use_ui_camera)
        {
            ui_blur_effect.EnableBlurRender(BlurType.Normal, data);
        }
        else
        {
            main_blur_effect.EnableBlurRender(BlurType.Normal, data);
        }
    }

    public void DisabledBlurCameraEffect(bool use_ui_camera)
    {
        if (use_ui_camera)
        {
            ui_blur_effect.DisabledBlurRender();
        }
        else
        {
            main_blur_effect.DisabledBlurRender();
        }
    }
}
