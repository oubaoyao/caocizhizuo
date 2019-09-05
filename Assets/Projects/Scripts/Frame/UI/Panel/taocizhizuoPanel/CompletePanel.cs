using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class CompletePanel : BasePanel
{
    public Button RemakeButtom, AppreciateButton, ReturnButton;
    public GamePanel gamepanel;
    public Animator starAniamtor;
    public RawImage DisplayRawImage;
    public Animation tiltle;
    public CanvasGroup zhuangshiCanvas;

    public override void InitFind()
    {
        base.InitFind();
        RemakeButtom = FindTool.FindChildComponent<Button>(transform, "BGImage/RemakeButtom");
        AppreciateButton = FindTool.FindChildComponent<Button>(transform, "BGImage/AppreciateButton");
        ReturnButton = FindTool.FindChildComponent<Button>(transform, "BGImage/ReturnButton");

        gamepanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");

        starAniamtor = FindTool.FindChildComponent<Animator>(transform, "BGImage/StarAnima");
        DisplayRawImage = FindTool.FindChildComponent<RawImage>(transform, "DisplayGroup/DisplayRaw");

        tiltle = FindTool.FindChildComponent<Animation>(transform, "tiltle");

        zhuangshiCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "DisplayGroup/zhaungshi");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        RemakeButtom.onClick.AddListener(() => {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            //GamePanel.CurrentModel.gameObject.SetActive(true);
            //ModelControl.Instance.ResetMaterial();
            gamepanel.Open();
        });

        AppreciateButton.onClick.AddListener(() => {
            Hide();
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.AppreciatePanel);
        });

        ReturnButton.onClick.AddListener(() => {
            Hide();
            TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        });
    }

    public override void Open()
    {
        base.Open();
        //Cursor.visible = false;
        starAniamtor.SetBool("newstate-starAnimation", true);
        starAniamtor.SetBool("starlooperanimation-newstate", false);
        DisplayRawImage.texture = gamepanel.CurrentDisplayTexture2D;
        tiltle.Play();
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 3.0f, Displayzhuangshi);
        AudioManager.PlayAudio("陶瓷制作-星星出现", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    }

    private void Displayzhuangshi()
    {
        zhuangshiCanvas.alpha = 1;
        AudioManager.PlayAudio("勋章-正确的声音2", transform, MTFrame.MTAudio.AudioEnunType.Effset);
    }

    public override void Hide()
    {
        base.Hide();
        AudioManager.StopAudio("陶瓷制作-星星出现", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        AudioManager.StopAudio("勋章-正确的声音2", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        starAniamtor.SetBool("newstate-starAnimation", false);
        starAniamtor.SetBool("starlooperanimation-newstate", true);
        tiltle.Stop();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, Displayzhuangshi);
        zhuangshiCanvas.alpha = 0;
    }

}
