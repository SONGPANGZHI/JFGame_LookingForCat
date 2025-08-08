using System;
using System.Net;
using UnityEngine;

public class BaseTools : MonoBehaviour
{
    public static BaseTools Instance = null;

    private Vector3 defualtCameraPos;       //默认位置
    private int width;
    private int height;
    private float defualtSize;
    public Camera MainSceneCamera;

    public static bool changePlaneSize;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #region 获得家具摄像机 调整相机深度大小

    //获取获取屏幕尺寸
    public void ScreenAdaptation(Camera camera)
    {
        MainSceneCamera = camera;
        width = Screen.width;
        height = Screen.height;
        defualtCameraPos = MainSceneCamera.transform.position;

        float screenRatio = (float)width / height;
        float designRatio = 1080f / 1980f;
        if (screenRatio >= designRatio)
        {
            //比例 designRatio = 0.5625
            MainSceneCamera.orthographicSize = 9;
            changePlaneSize = false;
        }
        else
        {
            // 窄屏设备
            MainSceneCamera.orthographicSize = 11;
            changePlaneSize = true;
        }

        defualtSize = MainSceneCamera.orthographicSize;
    }

    /// <summary>
    /// 设置摄像机显示
    /// </summary>
    /// <param name="type"></param>
    //public void SetCameraPosition(Vector2 _vector2, bool _ChangeSize)
    //{
    //    MainSceneCamera.transform.DOMove(new Vector3(_vector2.x, _vector2.y, -10f), 0.3f);
    //    if (_ChangeSize)
    //        MainSceneCamera.orthographicSize = 5;
    //    else
    //        MainSceneCamera.orthographicSize = defualtSize;
    //    //TutorCamera.gameObject.SetActive(type == 1);
    //}

    ////返回默认摄像机位置
    //public void RetureCameraDefualtPosition()
    //{
    //    MainSceneCamera.transform.DOMove(defualtCameraPos, 0.3f);
    //    MainSceneCamera.orthographicSize = defualtSize;
    //}

    #endregion

  

    //三消界面部分UI适配
    public void UIAdaptive(RectTransform _uiTrans, RectTransform _middle,RectTransform catPos)
    {
        width = Screen.width;
        height = Screen.height;
        double screenRatio = Math.Round((double)width / height, 4);
        double designRatio = Math.Round(1080f / 1980f, 4); 
        if (screenRatio >= designRatio)
        {
            //使用默认的
            _uiTrans.sizeDelta = new Vector2(0, 1500);
            _middle.sizeDelta = new Vector2(0, 680);
            catPos.sizeDelta = new Vector2(0,200);

        }
        else
        {
            //改变UI大小
            _uiTrans.sizeDelta = new Vector2(0, 1800);
            _middle.sizeDelta = new Vector2(0, 800);
            catPos.sizeDelta = new Vector2(0,520);
        }
    }


   
    public void GeneralTips(string _tipsContent)
    {
        //float X = Input.mousePosition.x - Screen.width / 2f;
        //float Y = Input.mousePosition.y - Screen.height / 2f;
        //Vector3 tranPos = new Vector3(X, Y, 0);

        //tipsObject.SetActive(true);
        //tipsObject.transform.localPosition = tranPos;
        //tipsObject.transform.Find("TipText").GetComponent<Text>().text = _tipsContent;

        //float defaultY = tipsObject.GetComponent<RectTransform>().anchoredPosition.y;

        //Sequence mySequence = DOTween.Sequence();
        //mySequence.Append(tipsObject.GetComponent<RectTransform>().DOAnchorPosY(defaultY + 235f, 2.5f));
        //mySequence.Join(tipsObject.transform.Find("TipText").GetComponent<Text>().DOColor(new Color32(255, 255, 255, 0), 2.5f));

        //mySequence.onComplete = () =>
        //{
        //    tipsObject.SetActive(false);
        //    tipsObject.transform.Find("TipText").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        //};
    }

    /// <summary>
    /// 获取UI适配参数
    /// </summary>
    /// <returns></returns>
    public float GetAdaptationScale()
    {
        float scaleParm;
        float ration = 2560f / 1440f;
        float d = ((float)Screen.height / (float)Screen.width);
        scaleParm = ration / d;
        scaleParm = Mathf.Min(scaleParm, 1);
        return scaleParm;
    }

    /// <summary>
    /// 判断url是否可用
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public bool IsCanConnect(string url)
    {
        HttpWebRequest req = null;
        HttpWebResponse res = null;
        bool CanCn = true;   //设成可以连接；
        try
        {
            req = (HttpWebRequest)WebRequest.Create(url);
            res = (HttpWebResponse)req.GetResponse();
        }
        catch (Exception)
        {
            CanCn = false;   //无法连接
        }
        finally
        {
            if (res != null)
            {
                res.Close();
            }
        }
        return CanCn;
    }

    /// <summary>
    /// 等比缩放精灵
    /// </summary>
    /// <param name="pendingSprite"></param>
    /// <param name="tagreatTrans"></param>
    public void SetFurnitureSize(Sprite pendingSprite, Transform tagreatTrans)
    {
        Vector3 pendingSize = pendingSprite.bounds.size;
        Vector2 tagreatRect = tagreatTrans.GetComponent<RectTransform>().sizeDelta;
        float pendingFloat;
        if (pendingSize.x > pendingSize.y)
        {
            pendingFloat = pendingSize.y / pendingSize.x;
            tagreatTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(tagreatRect.x, tagreatRect.y * pendingFloat);
        }
        else
        {
            pendingFloat = pendingSize.x / pendingSize.y;
            tagreatTrans.GetComponent<RectTransform>().sizeDelta = new Vector2(tagreatRect.x * pendingFloat, tagreatRect.y);
        }
    }

    public float AdaptationCamreaSize()
    {
        Debug.Log("device Screen height:   " + Screen.height);
        float cameraSize = (float)Screen.height / 2 / 100;
        return cameraSize;
    }

    #region 拍照保存

    //保存 保留照片 替换主界面图
    //public void CapturePhoto()
    //{
    //    string savePath = Path.Combine(Application.persistentDataPath, "LatestPhoto.png");
    //    StartCoroutine(CaptureAndSave(savePath));
    //}

    //private IEnumerator CaptureAndSave(string filePath)
    //{
    //    yield return new WaitForEndOfFrame();

    //    // 销毁旧纹理（避免内存泄漏）
    //    if (_currentPhoto != null)
    //    {
    //        Destroy(_currentPhoto);
    //    }

    //    // 创建新纹理
    //    RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
    //    MainSceneCamera.targetTexture = rt;
    //    _currentPhoto = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

    //    MainSceneCamera.Render();
    //    RenderTexture.active = rt;
    //    _currentPhoto.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
    //    _currentPhoto.Apply();

    //    // 清理RenderTexture
    //    MainSceneCamera.targetTexture = null;
    //    RenderTexture.active = null;
    //    Destroy(rt);

    //    // 保存到文件（覆盖旧文件）
    //    File.WriteAllBytes(filePath, _currentPhoto.EncodeToPNG());
    //    Debug.Log($"照片已更新: {filePath}");
    //}

    //// 获取当前照片的Texture2D（用于实时显示）
    //public Texture2D GetCurrentPhoto()
    //{
    //    // 确保路径有效
    //    if (string.IsNullOrEmpty("LatestPhoto.png"))
    //    {
    //        Debug.LogWarning("照片文件名为空！");
    //        return null;
    //    }

    //    string filePath = Path.Combine(Application.persistentDataPath, "LatestPhoto.png");

    //    // 检查文件是否存在
    //    if (!File.Exists(filePath))
    //    {
    //        Debug.Log($"未找到照片文件，路径: {filePath}");
    //        return CreateDefaultTexture(); // 返回默认纹理而不是null
    //    }

    //    try
    //    {
    //        byte[] fileData = File.ReadAllBytes(filePath);
    //        if (fileData == null || fileData.Length == 0)
    //        {
    //            Debug.LogWarning("照片文件为空或损坏");
    //            return CreateDefaultTexture();
    //        }

    //        Texture2D loadedPhoto = new Texture2D(2, 2);
    //        if (!loadedPhoto.LoadImage(fileData)) // 加载失败会返回false
    //        {
    //            Debug.LogWarning("照片加载失败，可能是不支持的格式");
    //            Destroy(loadedPhoto);
    //            return CreateDefaultTexture();
    //        }

    //        return loadedPhoto;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"加载照片时出错: {e.Message}");
    //        return CreateDefaultTexture();
    //    }
    //}

    //// 创建默认纹理（替代null）
    //private Texture2D CreateDefaultTexture()
    //{
    //    Texture2D defaultTex = new Texture2D(1, 1);
    //    defaultTex.SetPixel(0, 0, Color.gray); // 灰色默认纹理
    //    defaultTex.Apply();
    //    return defaultTex;
    //}


    #endregion

}
