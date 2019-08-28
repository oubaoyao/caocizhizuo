using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class Gametask : BaseTask
{
    public Gametask(BaseState state) : base(state) { }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<GamePanel>(WindowTypeEnum.ForegroundScreen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<GamePanel>(WindowTypeEnum.ForegroundScreen, UIPanelStateEnum.Hide);
    }
}
