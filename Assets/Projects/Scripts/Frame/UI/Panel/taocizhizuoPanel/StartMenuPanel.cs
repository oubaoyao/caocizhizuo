using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;

public class StartMenuPanel : BasePanel
{
    public Button PlayButton;
    public override void InitFind()
    {
        base.InitFind();
        PlayButton = FindTool.FindChildComponent<Button>(transform, "Button");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        PlayButton.onClick.AddListener(() =>
        {
            TCZZState.SwitchPanel(SwitchPanelEnum.GamePanel);
        });
    }
}
