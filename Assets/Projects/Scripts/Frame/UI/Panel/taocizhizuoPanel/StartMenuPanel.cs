using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;

public class StartMenuPanel : BasePanel
{
    public Button PlayButton;
    public Animator tiltleAnimator,StartButton;

    public override void InitFind()
    {
        base.InitFind();
        PlayButton = FindTool.FindChildComponent<Button>(transform, "Button");
        tiltleAnimator = FindTool.FindChildComponent<Animator>(transform, "TiltleImage");
        StartButton = FindTool.FindChildComponent<Animator>(transform, "Button");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        PlayButton.onClick.AddListener(() =>
        {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            StartButton.SetBool("start", false);
            StartButton.SetBool("stop", true);
            TCZZState.SwitchPanel(SwitchPanelEnum.GamePanel);
        });
    }

    public override void Open()
    {
        base.Open();
        tiltleAnimator.SetBool("newstate-tiltle", true);
        tiltleAnimator.SetBool("loopertiltle-Newstate", false);

        StartButton.SetBool("start", true);
        StartButton.SetBool("stop", false);

        AudioManager.StopAudio("Little West - Lost In My View-背景音乐", transform, MTFrame.MTAudio.AudioEnunType.BGM);
        AudioManager.PlayAudio("陶瓷制作-待机-财政司司长 gotjoy", transform, MTFrame.MTAudio.AudioEnunType.BGM, 0.25f, true);
    }

    public override void Hide()
    {
        base.Hide();
        tiltleAnimator.SetBool("newstate-tiltle", false);
        tiltleAnimator.SetBool("loopertiltle-Newstate", true);

        AudioManager.StopAudio("陶瓷制作-待机-财政司司长 gotjoy", transform, MTFrame.MTAudio.AudioEnunType.BGM);
        AudioManager.PlayAudio("Little West - Lost In My View-背景音乐", transform, MTFrame.MTAudio.AudioEnunType.BGM, 0.25f, true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
        }
    }
}
