using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class AppreciateTask : BaseTask
{
    public AppreciateTask(BaseState state):base(state)
    {

    }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<AppreciatePanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<AppreciatePanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
    }
}
