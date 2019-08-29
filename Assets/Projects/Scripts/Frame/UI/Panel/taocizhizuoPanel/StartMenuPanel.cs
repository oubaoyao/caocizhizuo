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
    }

    public override void Hide()
    {
        base.Hide();
        tiltleAnimator.SetBool("newstate-tiltle", false);
        tiltleAnimator.SetBool("loopertiltle-Newstate", true);
    }
}
