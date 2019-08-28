using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class StartMenutask : BaseTask
{
    public StartMenutask(BaseState state) : base(state) { }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<StartMenuPanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<StartMenuPanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
    }
}
