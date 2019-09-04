using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public AnimationCurve curve;
    public int index = 0;
    public float min = 0.5f;
    public float max = 1.4f;
    void Start ( )
    {
        curve = this.GetComponent<FlareDeformer>().Refinecurve;
        
    }

    // Update is called once per frame
    void Update ( )
    {
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    Extruction(index);
        //}
        //else if (Input.GetKey(KeyCode.E))
        //{
        //    Press (index);
        //}
    }

    //public void Extruction (int index )//挤出
    //{
    //    float currentValue = curve.keys[index].value;
    //    while (currentValue <= max)
    //    {
    //        float newValue = curve.keys[index].value + 0.01f;
    //        float newTime = curve.keys[index].time;
    //        Keyframe keyFrame = new Keyframe(time: newTime , newValue);
    //        curve.MoveKey(index , keyFrame);
    //        return;
    //    }
    //}
    public void Press (int index)//按压
    {
        float currentValue = curve.keys[index].value;
        while (currentValue >= min )
        {

            float newValue = curve.keys[index].value + 0.01f;
            float newTime = curve.keys[index].time;
            Keyframe keyFrame = new Keyframe(time: newTime , newValue);
            curve.MoveKey(index, keyFrame);
            return;
        }
    }
}
