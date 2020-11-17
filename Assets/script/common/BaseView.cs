using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BaseView : MonoBehaviour
{
    protected bool need_blur_bg = false;
    protected bool use_ui_blur = true;
    protected BlurType blur_type = BlurType.ScreenShot;
    GameObject bg_obj;
    RawImage bg_raw;
    RenderTexture blur_bg_rt;

    protected void Awake()
    {
        if(need_blur_bg)
        {
            // 构造默认的模糊数据
            BlurData blur_data= new BlurData();
            blur_data.blur_spread = 1;
            blur_data.blur_iteration = 4;
            blur_data.blur_size = 1;
            blur_data.blur_down_sample = 4;
            // 截屏式的模糊
            if(blur_type == BlurType.ScreenShot)
            {
                // 隐藏界面本身，因为界面本身不需要被拍入画面
                gameObject.SetActive(false);
                // 创建挂载模糊图片的节点，使用的是RawImage
                bg_obj = new GameObject("blur_bg");
                bg_obj.transform.SetParent(this.transform);
                bg_obj.transform.localScale = Vector3.one;
                bg_obj.transform.SetAsFirstSibling();
                bg_obj.AddComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
                Vector3 local_pos = bg_obj.GetComponent<RectTransform>().localPosition;
                bg_obj.GetComponent<RectTransform>().localPosition = new Vector3(local_pos.x, local_pos.y, 0);
                bg_raw = bg_obj.AddComponent<RawImage>();

                Action<RenderTexture> action = SetBlurImage;
                BlurEffectManager.Instance.EnableBlurScreenshot(use_ui_blur, blur_data, action);
            }
            // 实时模糊效果
            else if (blur_type == BlurType.Normal)
            {
                BlurEffectManager.Instance.EnableBlurCameraEffect(use_ui_blur, blur_data);
            }
        }
    }

    void SetBlurImage(RenderTexture rt)
    {
        if(this.gameObject != null)
        {
            blur_bg_rt = rt;
            bg_raw.texture = blur_bg_rt;
            gameObject.SetActive(true);
        }
        else
        {
            RenderTexture.ReleaseTemporary(rt);
        }
    }

    void OnDestroy()
    {
        if (blur_bg_rt != null)
            RenderTexture.ReleaseTemporary(blur_bg_rt);
        else if (blur_type == BlurType.Normal)
            BlurEffectManager.Instance.DisabledBlurCameraEffect(use_ui_blur);
    }
}
