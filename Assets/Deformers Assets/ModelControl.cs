﻿using System;
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

    public GameObject Model;
    public AnimationCurve curve;
    public List<CurveData> curveDatas = new List<CurveData>();
    private Vector3 first= Vector3.zero,secent=Vector3.zero;
    private int CurveNumber = -1;
    private float AreaSize = 0.15f;
    public bool IsGameStart;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        curve = Model.GetComponent<FlareDeformer>().Refinecurve;
        IsGameStart = false;
    }

    private void ChangeKey(int index,int IsAdd)
    {
        float Newvalue = curve.keys[index].value + 0.04f* IsAdd;
        if(Newvalue <= 0.75)
        {
            Debug.Log("value已经是最小，不能再小了！！");
            return;
        }
        if(Newvalue >= 1)
        {
            foreach ( CurveData item in curveDatas)
            {
                if(item.CenterKey.time.Equals(curve[index].time))
                {
                    curveDatas.Remove(item);
                    Debug.Log("移除该点，复原该区域===" + curveDatas.Count);
                    break;
                    
                }
            }
            Debug.Log("index==" + index);
            if(index - 1 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    curve.RemoveKey(index);
                }
            }
            else
            {
                if(index == curve.keys.Length-2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        curve.RemoveKey(index - 1);
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        curve.RemoveKey(index - 1);
                    }
                }
            }
            return;
            
        }
        float NewTime = curve.keys[index].time;
        Debug.Log("当前value得值====" + Newvalue);
        Keyframe centerkey2 = new Keyframe(time: NewTime, Newvalue);
        curve.MoveKey(index, centerkey2);
    }

    public void ResetModel()
    {
        if (curve.keys.Length - 2 <= 0)
            return;
        int curvelenth = curve.keys.Length - 2;
        for (int i = 0; i < curvelenth; i++)
        {
            curve.RemoveKey(1);
        }
        curveDatas.Clear();
        CurveNumber = -1;
        Model.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OpenModel()
    {
        Model.transform.gameObject.SetActive(true);
    }

    public void CloseModel()
    {
        Model.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGameStart)
        {
            Model.transform.Rotate(Vector3.up*25);

            if (Input.GetMouseButtonUp(0))
            {
                secent = Input.mousePosition;

                if (Mathf.Abs(first.x - secent.x) > Mathf.Abs(first.y - secent.y))
                {
                    if (secent.x < first.x && CurveNumber != -1)
                    {
                        ChangeKey(CurveNumber, -1);
                        Debug.Log("向左");
                    }

                    if (secent.x > first.x && CurveNumber != -1)
                    {
                        ChangeKey(CurveNumber, 1);
                        Debug.Log("向右");
                    }
                }
                else
                {
                    if (secent.y < first.y)
                    {
                        Vector3 temp = Model.transform.localScale;
                        temp -= new Vector3(0, 0.05f, 0);
                        if (temp.y < 1)
                        {
                            Model.transform.localScale = new Vector3(1.5f, 1.0f, 1.5f);
                        }
                        else
                        {
                            //transform.localScale = temp;
                            Model.transform.localScale = Vector3.Lerp(Model.transform.localScale, temp, 1.0f);
                        }
                    }

                    if (secent.y > first.y)
                    {
                        Vector3 temp = Model.transform.localScale;
                        temp += new Vector3(0, 0.05f, 0);
                        if (temp.y > 1.5f)
                        {
                            Model.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        }
                        else
                        {
                            Model.transform.localScale = Vector3.Lerp(Model.transform.localScale, temp, 1.0f);
                        }
                    }
                }

                first = secent;
                CurveNumber = -1;
                Debug.Log("鼠标抬起");
            }

            if (Input.GetMouseButtonDown(0))
            {
                first = Input.mousePosition;
                Ray mRay = Camera.main.ScreenPointToRay(first);
                RaycastHit mHit;
                if (Physics.Raycast(mRay, out mHit))
                {
                    //Debug.Log("hit Point" + mHit.point);
                    //Debug.Log("LocalPos:" + transform.InverseTransformPoint(mHit.point));
                    Vector3 relativeVector3 = Model.transform.InverseTransformPoint(mHit.point);

                    float Proportion = (relativeVector3.y - 0.2f) / (5.4f - 0.2f);
                    //Debug.Log(Proportion);
                    float Time_leftkey = Proportion - AreaSize;
                    float Time_rightkey = Proportion + AreaSize;

                    if (Time_leftkey <= 0)
                    {
                        Time_leftkey = 0;
                    }

                    if (Time_rightkey >= 1)
                    {
                        Time_rightkey = 1;
                    }

                    if (curveDatas.Count > 0)
                    {
                        for (int i = 0; i < curveDatas.Count; i++)
                        {
                            //首先判断是否点在某个已经有了得范围里
                            if (curveDatas[i].LeftKey.time <= Proportion && Proportion <= curveDatas[i].RightKey.time)
                            {

                                for (int j = 0; j < curve.keys.Length; j++)
                                {
                                    if (curve.keys[j].time.Equals(curveDatas[i].CenterKey.time))
                                    {
                                        CurveNumber = j;
                                        Debug.Log("中心点已经存在");
                                        return;
                                    }
                                }
                            }

                            //判断左右两个key是否会生成在已有范围里
                            if ((curveDatas[i].LeftKey.time <= Time_leftkey && Time_leftkey <= curveDatas[i].RightKey.time) ||
                                (curveDatas[i].LeftKey.time <= Time_rightkey && Time_rightkey <= curveDatas[i].RightKey.time))
                            {
                                Debug.Log("左右点已经存在");
                                return;
                            }
                        }
                    }

                    Keyframe leftKey = new Keyframe(Time_leftkey, 1.0f);
                    leftKey.tangentMode = 0x101 | 0x10001;
                    Keyframe rightKey = new Keyframe(Time_rightkey, 1.0f);
                    rightKey.tangentMode = 0x101 | 0x10001;
                    Keyframe centerKey = new Keyframe(Proportion, 1.0f);
                    centerKey.tangentMode = 10;

                    curve.AddKey(leftKey);
                    curve.AddKey(rightKey);
                    curve.AddKey(centerKey);

                    CurveData curveData = new CurveData();
                    curveData.LeftKey = leftKey;
                    curveData.RightKey = rightKey;
                    curveData.CenterKey = centerKey;

                    curveDatas.Add(curveData);
                    Debug.Log("添加新的点");

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            OpenModel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CloseModel();
        }
    }


}