using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class CompletePanel : BasePanel
{
    public Button RemakeButtom, AppreciateButton, ReturnButton;
    public GamePanel gamepanel;
    public Animator starAniamtor;
    public RawImage DisplayRawImage;

    public override void InitFind()
    {
        base.InitFind();
        RemakeButtom = FindTool.FindChildComponent<Button>(transform, "BGImage/RemakeButtom");
        AppreciateButton = FindTool.FindChildComponent<Button>(transform, "BGImage/AppreciateButton");
        ReturnButton = FindTool.FindChildComponent<Button>(transform, "BGImage/ReturnButton");

        gamepanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");

        starAniamtor = FindTool.FindChildComponent<Animator>(transform, "BGImage/StarAnima");
        DisplayRawImage = FindTool.FindChildComponent<RawImage>(transform, "DisplayGroup/DisplayRaw");
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
    }

    public override void Hide()
    {
        base.Hide();
        starAniamtor.SetBool("newstate-starAnimation", false);
        starAniamtor.SetBool("starlooperanimation-newstate", true);
    }

}
