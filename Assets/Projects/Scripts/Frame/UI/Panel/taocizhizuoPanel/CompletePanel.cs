using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class CompletePanel : BasePanel
{
    public Button RemakeButtom, AppreciateButton, ReturnButton;
    public GamePanel gamepanel;

    public override void InitFind()
    {
        base.InitFind();
        RemakeButtom = FindTool.FindChildComponent<Button>(transform, "BGImage/RemakeButtom");
        AppreciateButton = FindTool.FindChildComponent<Button>(transform, "BGImage/AppreciateButton");
        ReturnButton = FindTool.FindChildComponent<Button>(transform, "BGImage/ReturnButton");

        gamepanel = FindTool.FindParentComponent<GamePanel>(transform, "GamePanel");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        RemakeButtom.onClick.AddListener(() => {
            Hide();
            
            //GamePanel.CurrentModel.gameObject.SetActive(true);
            //ModelControl.Instance.ResetMaterial();
            gamepanel.Open();
        });

        AppreciateButton.onClick.AddListener(() => {
            Hide();
            TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.AppreciatePanel);
        });

        ReturnButton.onClick.AddListener(() => {
            Hide();
            TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
        });
    }

    public override void Open()
    {
        base.Open();
        //Cursor.visible = false;
    }

}
