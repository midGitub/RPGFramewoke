using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestMediator : BaseMediator {
    private GameObject updateResBtn, updateScriptBtn, loadConfigBtn, testPoolBtn;
    private GameObject infoText;

    protected override void AddEvent()
    {
        updateResBtn=transform.Find("Btn1").gameObject;
        EventTriggerListener.Get(updateResBtn).PointerClick += HotUpdateResource;
        updateScriptBtn = transform.Find("Btn2").gameObject;
        EventTriggerListener.Get(updateScriptBtn).PointerClick += HotUpdateResource;
        loadConfigBtn = transform.Find("Btn3").gameObject;
        EventTriggerListener.Get(loadConfigBtn).PointerClick += HotUpdateResource;
        testPoolBtn = transform.Find("Btn4").gameObject;
        EventTriggerListener.Get(testPoolBtn).PointerClick += HotUpdateResource;
        infoText = transform.Find("Text").gameObject;
    }

    public void HotUpdateResource(PointerEventData eventData)
    {
        AssetBundleService.getInstance().LoadAsset("prefabs/object/cube.unity3d", "cube", testPrefabs);
    }

    public void HotUpdateScript(PointerEventData eventData)
    {
        object[] results = LuaManager.getInstance().CallMethod("GameManager", "hello");
        infoText.GetComponent<Text>().text = results[0].ToString();
    }

    public void LoadTxt(PointerEventData eventData)
    {
        CSkillLoader skillLoader = CStaticDownload<CSkillLoader>.getInstance().GetStaticInfo(100);
        GLog.Log(skillLoader.GetSkillName());
    }

    public void TestObjectPoll()
    {
        GameObject capsule = ObjectPool.instance.GetObjByName("Prefabs/Capsule");
        GameObject cube = ObjectPool.instance.GetObjByName("Prefabs/Cube");
        int range1 = Random.Range(-4, 0);
        capsule.transform.position = new Vector3(range1, range1, range1);
        int range2 = Random.Range(0, 4);
        cube.transform.position = new Vector3(range2, range2, range2);
    }

    private void testPrefabs(string assetName, UnityEngine.Object obj)
    {
        GameObject go =GameObject.Instantiate(obj as GameObject);
        go.transform.position = Vector3.zero;
    }
}
