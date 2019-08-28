using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using MTFrame.MTEvent;

public class TCZZState : BaseState
{
    public const string SwitchPanelEvent = "SwitchPanelEvent";

    public override string[] ListenerMessageID
    {
        get
        {
            return new string[]
            {
                SwitchPanelEvent,
            };
        }
        set { }
    }

    public override void OnListenerMessage(EventParamete parameteData)
    {
        if (parameteData.EvendName == SwitchPanelEvent)
        {
            switch (parameteData.GetParameter<SwitchPanelEnum>()[0])
            {
                case SwitchPanelEnum.StartMenuPanel:
                    CurrentTask.ChangeTask(new StartMenutask(this));
                    break;
                case SwitchPanelEnum.GamePanel:
                    CurrentTask.ChangeTask(new Gametask(this));
                    break;
                case SwitchPanelEnum.AppreciatePanel:
                    CurrentTask.ChangeTask(new AppreciateTask(this));
                    break;
            }
        }
    }

    public override void Enter()
    {
        base.Enter();
        CurrentTask.ChangeTask(new StartMenutask(this));
    }


    public static void SwitchPanel(SwitchPanelEnum st)
    {
        EventParamete eventParamete = new EventParamete();
        eventParamete.AddParameter(st);
        EventManager.TriggerEvent(GenericEventEnumType.Message, SwitchPanelEvent, eventParamete);
        Debug.Log("切换面板===" + st.ToString());
    }
}
