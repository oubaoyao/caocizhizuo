using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class WorksDataGroup
{
    public string[] worksDatas;
}

public class WorksDataControl : MonoBehaviour
{
    public static WorksDataControl Instance;
    public List<Texture2D> WorksDisplayTexture = new List<Texture2D>();
    public List<string> WorksDisplayPath = new List<string>();
    private string WorksJsonDataPath = "/WorksDatas";
    private string WorksJsonDataName = "WorksJsonDatas.Json";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.File.Exists(Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName))
        {
            //Debug.Log("读取到文件WorksJsonDatas");
            //string str = Resources.Load<TextAsset>("WorksDatas/WorksJsonDatas").text;
            string str = File.ReadAllText(Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName);
            WorksDataGroup worksDatasGroup = JsonConvert.DeserializeObject<WorksDataGroup>(str);
            for (int i = 0; i < worksDatasGroup.worksDatas.Length; i++)
            {
                WorksDisplayPath.Add(worksDatasGroup.worksDatas[i]);
                WorksDisplayTexture.Add(LoadByIO(Application.streamingAssetsPath + "/saveImage/" + worksDatasGroup.worksDatas[i] + ".jpg"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 30.0f, BackMainMenu);

        }

        if (Input.GetMouseButtonDown(0))
        {
            TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, BackMainMenu);
        }
    }

    private void BackMainMenu()
    {
        TCZZState.SwitchPanel(MTFrame.MTEvent.SwitchPanelEnum.StartMenuPanel);
        ModelControl.Instance.CloseModel();
        GC.Collect();
    }

    public void SaveFile(string msg, string FilePath)
    {
        var fss = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var sws = new System.IO.StreamWriter(fss);
        sws.Write(msg);
        sws.Close();
        fss.Close();
        Debug.Log("保存数据成功===" + msg);
    }

    private void OnDestroy()
    {
        if (WorksDisplayPath.Count > 0)
        {
            WorksDataGroup worksDataGroup = new WorksDataGroup();
            worksDataGroup.worksDatas = WorksDisplayPath.ToArray();
            string str = JsonConvert.SerializeObject(worksDataGroup);
            SaveFile(str, Application.streamingAssetsPath + WorksJsonDataPath + "/" + WorksJsonDataName);
        }
    }

    public Texture2D LoadByIO(string path)
    {
        //创建文件读取流
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        //创建Texture
        int width = 256;
        int height = 256;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);
        return texture2D;
    }

    public void DeleteTexture()
    {
        string str = Application.streamingAssetsPath + "/" + "saveImage" + "/" + WorksDisplayPath[0] + ".jpg";
        if(File.Exists(str))
        {
            File.Delete(str);
        }
        WorksDisplayTexture.RemoveAt(0);
        WorksDisplayPath.RemoveAt(0);
    }
}
