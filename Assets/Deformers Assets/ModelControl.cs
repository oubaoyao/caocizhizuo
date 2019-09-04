using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[System.Serializable]
public class CurveData
{
    public Keyframe LeftKey;
    public Keyframe RightKey;
    public Keyframe CenterKey;
}


public class ModelControl : MonoBehaviour
{
    public static ModelControl Instance;

    public GameObject Model,dipan;
    public AnimationCurve curve;
    public List<CurveData> curveDatas = new List<CurveData>();
    private Vector3 first= Vector3.zero,secent=Vector3.zero;
    private int CurveNumber = -1,EdgeNumber = -1;
    private float AreaSize = 0.2f;
    public bool IsGameStart,IsRight,IsLeft;

    public float min = 0.5f;
    public float max = 1.4f;

    private float[] KeyGroup = { 0, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        curve = Model.GetComponent<FlareDeformer>().Refinecurve;
        IsGameStart = IsRight = IsLeft = false;
        for (int i = 0; i < KeyGroup.Length; i++)
        {
            Keyframe keyframe = new Keyframe(KeyGroup[i], 1);
            keyframe.tangentMode = 10;
            curve.AddKey(keyframe);
        }
    }

    public void ResetModel()
    {
        for (int i = 0; i < KeyGroup.Length; i++)
        {
            curve.MoveKey(i, new Keyframe(KeyGroup[i], 1));
        }
        Model.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OpenModel()
    {
        Model.transform.gameObject.SetActive(true);
        dipan.transform.gameObject.SetActive(true);
    }

    public void CloseModel()
    {
        Model.transform.gameObject.SetActive(false);
        dipan.transform.gameObject.SetActive(false);
    }

    public void Press(int index,float value = 0.01f,float area = 0.5f)//按压
    {
        float currentValue = curve.keys[index].value;
        while (currentValue >= area)
        {

            float newValue = curve.keys[index].value - value;
            float newTime = curve.keys[index].time;
            Keyframe keyFrame = new Keyframe(time: newTime, newValue);
            curve.MoveKey(index, keyFrame);
            return;
        }
    }

    public void Extruction(int index, float value = 0.01f, float area = 1.4f)//挤出
    {
        float currentValue = curve.keys[index].value;
        while (currentValue <= area)
        {
            float newValue = curve.keys[index].value + value;
            float newTime = curve.keys[index].time;
            Keyframe keyFrame = new Keyframe(time: newTime, newValue);
            curve.MoveKey(index, keyFrame);
            return;
        }
    }


    private Vector3 oldPos, newPos;
    // Update is called once per frame
    void Update()
    {
        if(IsGameStart)
        {
            Model.transform.Rotate(Vector3.up*25);
            dipan.transform.Rotate(Vector3.forward * 25);

            newPos = Input.mousePosition;
            if (oldPos == Vector3.zero)
            {

            }
            else
            {
                if (Mathf.Abs(newPos.x - oldPos.x) > Mathf.Abs(newPos.y - oldPos.y))
                {
                    //Debug.Log("左右比上下幅度大");
                    if (newPos.x > oldPos.x)
                    {
                        //Debug.Log("向右");
                        if (CurveNumber != -1)
                        {
                            if (IsLeft == true)
                            {
                                Press(CurveNumber);
                                Press(CurveNumber - 1, 0.005f,0.75f);
                                Press(CurveNumber + 1, 0.005f, 0.75f);
                            }
                            else if (IsRight == true)
                            {
                                Extruction(CurveNumber);
                                Extruction(CurveNumber - 1, 0.005f,1.2f);
                                Extruction(CurveNumber + 1, 0.005f,1.2f);
                            }
                        }
                        if (EdgeNumber != -1)
                        {
                            if (IsLeft == true)
                            {
                                Press(EdgeNumber);
                            }
                            else if (IsRight == true)
                            {
                                Extruction(EdgeNumber);
                            }
                        }
                    }
                    else if (newPos.x < oldPos.x)
                    {
                        //Debug.Log("向左");
                        if (CurveNumber != -1)
                        {
                            if (IsLeft == true)
                            {
                                Extruction(CurveNumber);
                                Extruction(CurveNumber - 1, 0.005f,1.2f);
                                Extruction(CurveNumber + 1, 0.005f,1.2f);
                            }
                            else if (IsRight == true)
                            {
                                Press(CurveNumber);
                                Press(CurveNumber - 1, 0.005f,0.75f);
                                Press(CurveNumber + 1, 0.005f,0.75f);
                            }
                        }
                        if (EdgeNumber != -1)
                        {
                            if (IsLeft == true)
                            {
                                Extruction(EdgeNumber);
                            }
                            else if (IsRight == true)
                            {
                                Press(EdgeNumber);
                            }
                        }
                    }
                }
                else if (Mathf.Abs(newPos.x - oldPos.x) < Mathf.Abs(newPos.y - oldPos.y))
                {
                    //Debug.Log("上下比左右幅度大");
                    if (newPos.y > oldPos.y)
                    {
                        //Debug.Log("向上");
                        if (Model.transform.localScale.y > 1.5f)
                        {
                            Debug.Log("已经最大高度");
                        }
                        else
                        {
                            Model.transform.localScale += new Vector3(0, 0.005f, 0);
                        }
                    }
                    else if (newPos.y < oldPos.y)
                    {
                        //Debug.Log("向下");
                        if (Model.transform.localScale.y < 0.6)
                        {

                            Debug.Log("已经最小高度");
                        }
                        else
                        {
                            Model.transform.localScale -= new Vector3(0, 0.005f, 0);
                        }
                    }
                }
                //Debug.Log("newPos== " + newPos);
                //Debug.Log("oldPos==" + oldPos);
                //Debug.Log("chazhi===" + (newPos - oldPos));
            }
            oldPos = newPos;

            if (Input.GetMouseButtonUp(0))
            {
                CurveNumber = -1;
                EdgeNumber = -1;
                IsRight = IsLeft = false;
                //Debug.Log("鼠标抬起");
            }

            if (Input.GetMouseButtonDown(0))
            {
                first = Input.mousePosition;
                //Debug.Log("1111");
                Ray mRay = Camera.main.ScreenPointToRay(first);
                RaycastHit mHit;
                if (Physics.Raycast(mRay, out mHit))
                {
                    //Debug.Log("hit Point" + mHit.point);
                    //Debug.Log("LocalPos:" + Model.transform.InverseTransformPoint(mHit.point));
                    if (mHit.point.x > 0)
                    {
                        IsRight = true;
                        //Debug.Log("IsRight==" + IsRight);
                    }
                    else if(mHit.point.x < 0)
                    {
                        IsLeft = true;
                        //Debug.Log("IsLeft==" + IsLeft);
                    }
                    Vector3 relativeVector3 = Model.transform.InverseTransformPoint(mHit.point);

                    float Proportion = (relativeVector3.y - 0.2f) / (5.4f - 0.2f);

                    if(Proportion >= 0.95 )
                    {
                        EdgeNumber = curve.keys.Length - 1;
                        return;
                    }
                    else if(Proportion < 0.15)
                    {
                        EdgeNumber = 0;
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < KeyGroup.Length; i++)
                        {
                            if(i!=0&&i!=KeyGroup.Length-1)
                            {
                                if(Proportion - KeyGroup[i] <= 0.05)
                                {
                                    CurveNumber = i;
                                    Debug.Log("当前区域===" + i);
                                    return;
                                }
                            }
                        }
                    }

                }
            }
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    OpenModel();
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    CloseModel();
        //}
    }


}
