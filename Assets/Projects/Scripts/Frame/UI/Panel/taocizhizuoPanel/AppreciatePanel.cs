using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class AppreciatePanel : BasePanel
{
    public Button  Image_Right_Button, Image_Left_Button, BackButton;
    public Button[] ImageButtonGroup;
    public RawImage[] ImageGroup;
    private Texture[] WorksDisplayTextureArray;
    public Transform ChooseIngImage;
    public float[] ChooseIngImageX = { -268f, -133.28f, 1.0f, 134.1f, 267.7f };
    private RawImage DisplayRawImage;

    private int Index = 0;

    public override void InitFind()
    {
        base.InitFind();

        Image_Right_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Right_Button");
        Image_Left_Button = FindTool.FindChildComponent<Button>(transform, "Buttons/Image_Left_Button");
        BackButton = FindTool.FindChildComponent<Button>(transform, "Buttons/BackButton");

        ImageButtonGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Button>();
        ImageGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<RawImage>();
        ChooseIngImage = FindTool.FindChildNode(transform, "ChooseIng");

        DisplayRawImage = FindTool.FindChildComponent<RawImage>(transform, "xiangkuang/DisplayImage");
    }

    public override void InitEvent()
    {
        base.InitEvent();

        Image_Right_Button.onClick.AddListener(() => {
            Right();
        });

        Image_Left_Button.onClick.AddListener(() => {
            Left();
        });

        BackButton.onClick.AddListener(() => {
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);

        });
    }

    public override void Open()
    {
        base.Open();
        ChooseIngImage.localPosition = new Vector3(-268f, -372f);
        Index = 0;

        if (WorksDataControl.Instance.WorksDisplayTexture.Count > 0)
        {
            WorksDisplayTextureArray = null;
            WorksDisplayTextureArray = WorksDataControl.Instance.WorksDisplayTexture.ToArray();

            if (WorksDisplayTextureArray.Length != 0)
            {
                for (int i = 0; i < ImageGroup.Length; i++)
                {
                    if (i < WorksDisplayTextureArray.Length)
                    {
                        ImageGroup[i].texture = WorksDisplayTextureArray[i];
                    }
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
            DisplayRawImage.texture = WorksDisplayTextureArray[0];
        }

    }

    void InitButtons(Button btn, int i, int index)
    {
        btn.onClick.AddListener(delegate () {
            
            if(i + index < WorksDisplayTextureArray.Length && WorksDisplayTextureArray != null)
            {
                DisplayRawImage.texture = WorksDisplayTextureArray[i + index];
            }
            AudioManager.PlayAudio("按键声音", transform, MTFrame.MTAudio.AudioEnunType.Effset);
            ChooseIngImage.localPosition = new Vector3(ChooseIngImageX[i], -372f, 0);
        });
    }

    public void ImageAddListen(Button[] buttons, int index)
    {
        foreach (Button item in buttons)
        {
            item.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < buttons.Length; i++)
        {
            InitButtons(buttons[i], i, index);
        }
    }

    public void Left()
    {
        if (WorksDisplayTextureArray.Length != 0)
        {
            Index--;
            if (Index < 0)
            {
                Index = 0;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksDisplayTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksDisplayTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

    }

    public void Right()
    {
        if (WorksDisplayTextureArray.Length != 0)
        {
            Index++;
            if (Index + ImageGroup.Length > WorksDisplayTextureArray.Length)
            {
                Index--;
                return;
            }
            for (int i = 0; i < ImageGroup.Length; i++)
            {
                if (i < WorksDisplayTextureArray.Length)
                {
                    ImageGroup[i].texture = WorksDisplayTextureArray[i + Index];
                }
            }
            ImageAddListen(ImageButtonGroup, Index);
        }

    }
}
