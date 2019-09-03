using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;

public class GamePanel : BasePanel
{
    public Button BackButton, ConcirmButton, tips1Button, tips2Button;
    public CanvasGroup tips1, tips2, Handtips;
    private string SaveImaPath = "saveImage";
    public CompletePanel completePanel;

    public override void InitFind()
    {
        base.InitFind();
        BackButton = FindTool.FindChildComponent<Button>(transform, "buttons/BackButton");
        ConcirmButton = FindTool.FindChildComponent<Button>(transform, "buttons/ConcirmButton");

        tips1 = FindTool.FindChildComponent<CanvasGroup>(transform, "tips/tips1");
        tips2 = FindTool.FindChildComponent<CanvasGroup>(transform, "tips/tips2");
        Handtips = FindTool.FindChildComponent<CanvasGroup>(transform, "tips/Handtips");

        tips1Button = tips1.gameObject.GetComponent<Button>();
        tips2Button = tips2.gameObject.GetComponent<Button>();

        completePanel = FindTool.FindChildComponent<CompletePanel>(transform, "CompletePanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        BackButton.onClick.AddListener(() => {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            TCZZState.SwitchPanel(SwitchPanelEnum.StartMenuPanel);
        });

        ConcirmButton.onClick.AddListener(() => {
            CloseTips();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            ModelControl.Instance.IsGameStart = false;
            string ImgName = Time.time.ToString();
            WorksDataControl.Instance.WorksDisplayPath.Add(ImgName);
            string str = Application.streamingAssetsPath + "/" + SaveImaPath + "/" + ImgName + ".jpg";
            StartCoroutine(getScreenTexture(str));
            completePanel.Open();
        });

        tips1Button.onClick.AddListener(() => {
            tips1.DOFillAlpha(0, 0.5f, TweenMode.NoUnityTimeLineImpact).OnComplete(()=> {
                if (tips2.alpha == 0)
                {
                    Handtips.gameObject.GetComponent<Animation>().Stop();
                    Handtips.DOFillAlpha(0, 0.5f, TweenMode.NoUnityTimeLineImpact);
                }
            });
            tips1.blocksRaycasts = false;

        });

        tips2Button.onClick.AddListener(() => {
            tips2.DOFillAlpha(0, 0.5f, TweenMode.NoUnityTimeLineImpact).OnComplete(()=> {
                if (tips1.alpha == 0)
                {
                    Handtips.gameObject.GetComponent<Animation>().Stop();
                    Handtips.DOFillAlpha(0, 0.5f, TweenMode.NoUnityTimeLineImpact);
                }
            });
            tips2.blocksRaycasts = false;

        });
    }

    public override void Open()
    {
        base.Open();
        OpenTips();
        ModelControl.Instance.IsGameStart = true;
        ModelControl.Instance.ResetModel();
        ModelControl.Instance.OpenModel();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, CloseTips);
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 15, CloseTips);
    }

    public override void Hide()
    {
        base.Hide();
        
        ModelControl.Instance.IsGameStart = false;
        ModelControl.Instance.CloseModel();
        CloseTips();
    }

    private void OpenTips()
    {
        tips1.alpha = 1;
        tips1.blocksRaycasts = true;

        tips2.alpha = 1;
        tips2.blocksRaycasts = true;

        Handtips.alpha = 1;
        Handtips.gameObject.GetComponent<Animation>().Play();
    }

    private void CloseTips()
    {
        tips1.alpha = 0;
        tips1.blocksRaycasts = false;

        tips2.alpha = 0;
        tips2.blocksRaycasts = false;

        Handtips.alpha = 0;
        Handtips.gameObject.GetComponent<Animation>().Stop();
    }

    IEnumerator getScreenTexture(string path)
    {
        yield return new WaitForEndOfFrame();
        //需要正确设置好图片保存格式
        Texture2D t = new Texture2D(600, 600, TextureFormat.RGB24, false);
        //按照设定区域读取像素；注意是以左下角为原点读取
        t.ReadPixels(new Rect(0.2f * Screen.width, 0.32f * Screen.height, 600, 600), 0, 0);
        t.Apply();
        WorksDataControl.Instance.WorksDisplayTexture.Add(t);
        if(WorksDataControl.Instance.WorksDisplayTexture.Count > 15)
        {
            WorksDataControl.Instance.DeleteTexture();
        }
        //二进制转换
        byte[] byt = t.EncodeToJPG();
        System.IO.File.WriteAllBytes(path, byt);
        ModelControl.Instance.CloseModel();
    }

    
}
