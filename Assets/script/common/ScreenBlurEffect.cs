using System;
using UnityEngine;
public enum BlurType{
    Normal = 0,
    ScreenShot = 1,
}
public class BlurData{
    public float blur_size;
    public int blur_iteration;
    public int blur_down_sample;
    public float blur_spread;
}

[RequireComponent(typeof(Camera))]
public class ScreenBlurEffect : MonoBehaviour
{
    // 预先定义shader渲染用的pass
    const int BLUR_HOR_PASS = 0;
    const int BLUR_VER_PASS = 1;
    bool is_support; // 判断当前平台是否支持模糊
    
    RenderTexture final_blur_rt;
    RenderTexture temp_rt;
    [SerializeField]
    public Material blur_mat; // 模糊材质球

    // 外部参数
    [Range(0, 127)]
    float blur_size = 1.0f; // 模糊额外散步大小
    [Range(1, 10)]
    public int blur_iteration = 4; // 模糊采样迭代次数
    public float blur_spread = 1; // 模糊散值
    int cur_iterate_num = 1; // 当前迭代次数
    public int blur_down_sample = 4; // 模糊初始降采样比率
    public bool render_blur_effect = false; // 是否开始渲染模糊效果
    public bool render_blur_screenShot = false; // 模糊截图执行开关
    private Action<RenderTexture> blur_callback;
    
    void Awake()
    {
        is_support = SystemInfo.supportsImageEffects;
    }

    // 模糊后处理的主要方法 
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(is_support && blur_mat != null && (render_blur_effect || render_blur_screenShot)){
            // 首先对输出的结果做一次降采样，也就是降低分辨率，减小RT图的大小
            int width = src.width / blur_down_sample;
            int height = src.height / blur_down_sample;
            // 将当前摄像机画面渲染到被降采样的RT上
            final_blur_rt = RenderTexture.GetTemporary(width, height, 0);
            Graphics.Blit(src, final_blur_rt);

            cur_iterate_num = 1; // 初始化迭代
            while(cur_iterate_num <= blur_iteration)
            {
                blur_mat.SetFloat("_BlurSize", (1.0f + cur_iterate_num * blur_spread) * blur_size);  // 设置模糊扩散uv偏移
                temp_rt = RenderTexture.GetTemporary(width, height, 0);  
                // 使用blit的其他重载，针对对应的材质球和pass进行渲染并输出结果
                Graphics.Blit(final_blur_rt, temp_rt, blur_mat, BLUR_HOR_PASS);
                Graphics.Blit(temp_rt, final_blur_rt, blur_mat, BLUR_VER_PASS);
                RenderTexture.ReleaseTemporary(temp_rt);   // 释放临时RT
                cur_iterate_num ++;
            }
            // 如果只是渲染截图
            if(render_blur_screenShot && !render_blur_effect){
                GetBlurScreenShot();
                Graphics.Blit(src, dest); // 不修改最终输出画面
                RenderTexture.ReleaseTemporary(final_blur_rt);  // final_blur_rt作用已经完成，可以回收了
                DisabledBlurRender(); // 截图逻辑执行完毕后就关闭脚本
            }else // 其他情况一律处理为动态模糊背景
            {
                Graphics.Blit(final_blur_rt, dest);
                RenderTexture.ReleaseTemporary(final_blur_rt);  // final_blur_rt作用已经完成，可以回收了
            }
        }
        else{
            Graphics.Blit(src, dest);
        }
    }

    public void EnableBlurRender(BlurType blur_type, BlurData data = null, Action<RenderTexture> callback = null)
    {
        blur_size = data != null ? data.blur_size : 1.0f;
        blur_iteration = data != null ? data.blur_iteration : 4;
        blur_down_sample = data != null ? data.blur_down_sample : 4;
        blur_spread = data != null ? data.blur_spread : 1;

        if(blur_type == BlurType.Normal)
        {
            render_blur_effect = true;
        }
        else if (blur_type == BlurType.ScreenShot) 
        {
            render_blur_screenShot = true;
        }
        
        blur_callback = callback;
        this.enabled = true;
    }
    // 禁用渲染
    public void DisabledBlurRender()
    {
        render_blur_effect = false;
        render_blur_screenShot = false;
        this.enabled = false;
    }
    void GetBlurScreenShot()
    {
        if(blur_callback != null)
        {
            RenderTexture temp_screen_shot = RenderTexture.GetTemporary(final_blur_rt.width, final_blur_rt.height, 0);
            Graphics.Blit(final_blur_rt, temp_screen_shot);
            // 调用传入的回调
            blur_callback(temp_screen_shot);
        }
        // 无论执行与否，都要清除一次回调引用
        blur_callback = null;
    }
}
