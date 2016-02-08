using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using System;

public class AnimatorEditor : Editor
{

    #region 私有变量
    /// <summary>
    /// animatorcontroller默认状态
    /// </summary>
    private static string defaultStateName = "idle";
    private static Dictionary<int, AnimatorState> combos;
    /// <summary>
    /// 静态文件存放文件夹
    /// </summary>
    private static string dirName = "StaticDatas";
    /// <summary>
    /// 技能事件表名
    /// </summary>
    private static string configName = "skillEvent.txt";
    private static List<SkillEvent> skillEvents;
    #endregion

    class SkillEvent
    {
        public int id;
        public string name;
        public string controllerName;
        public List<int> keys;
        public string animName;
        public List<string> methodsName;
        public Dictionary<int, string> parametes;
    }

    #region 编辑器方法
    /// <summary>
    /// 根据动画文件生成Animator Controller
    /// </summary>
    [MenuItem("Tools/Animator/Create AnimatorController")]
    public static void CreateAnimatorController() {
        AnimatorController controller = null;
        combos = new Dictionary<int, AnimatorState>();
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
            string path = AssetDatabase.GetAssetPath(obj);
            string[] str=path.Split('/');
            controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Animator/" + str[str.Length-1]+".controller");
        }
        if (controller == null)
            return;
       foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
       {
           string path = AssetDatabase.GetAssetPath(obj);
           if (!path.Contains(".FBX") || !path.Contains("@"))
               continue;
           AddParameter(path, controller);
       }
       AnimatorStateMachine rootMachine = controller.layers[0].stateMachine;
       AnimatorState defaultState = rootMachine.defaultState;
       ChildAnimatorState[] states = rootMachine.states;
       foreach (ChildAnimatorState state in states)
       {
           if (!rootMachine.defaultState.name.Equals(state.state.name))
           {
               //idle到其他state的condition
               AnimatorStateTransition transitionFrom = defaultState.AddTransition(state.state);
               transitionFrom.AddCondition(AnimatorConditionMode.If, 0f, state.state.name);
               transitionFrom.duration = 0;
               transitionFrom.exitTime = 0;
               //其他state到idle的condition
               AnimatorStateTransition transitionTo = state.state.AddTransition(defaultState);
               transitionTo.AddCondition(AnimatorConditionMode.IfNot, 0f, state.state.name);
               transitionTo.duration = 0;
               transitionTo.exitTime = 0;

               //设置连招
               //if ("atk1".Equals(state.state.name))
               //    combos.Add(1,state.state);
               //else if ("atk2".Equals(state.state.name))
               //    combos.Add(2, state.state);
               //else if ("atk3".Equals(state.state.name))
               //    combos.Add(3, state.state);
               //else if ("atk4".Equals(state.state.name))
               //    combos.Add(4, state.state);
           }
           //BindBehaviour(state.state);
       }
       //ConnectCombo(combos);
    }

    /// <summary>
    /// 读表绑定动画事件
    /// </summary>
    [MenuItem("Tools/Animator/BindEvent")]
    public static void BindEvent()
    {
        skillEvents = null;
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets)) {
            string filePath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(filePath) && filePath.EndsWith(".FBX"))
            {
                string fileName = filePath.Substring(filePath.LastIndexOf('/')+1);
                string[] result=fileName.Split('.')[0].Split('@');
                if (result.Length < 2)
                    continue;
                string controllerName = result[0];
                string animName = result[1];             
                if(skillEvents==null)
                    LoadFile(Application.dataPath + "/" + dirName + "/" + configName);
                SkillEvent se=skillEvents.Find(x=>x.controllerName.Equals(controllerName) && x.animName.Equals(animName));
                if (se==null)
                    continue;
                Debug.Log("controllerName is:" + controllerName + "---animName is:" + animName);
                Dictionary<int, string> ps = se.parametes;
                List<string> methodNames=se.methodsName;
                List<int> keys=se.keys;
                ModelImporter modelImporter = (ModelImporter)ModelImporter.GetAtPath(filePath);
                if (modelImporter.defaultClipAnimations[0] == null) return;
                ModelImporterClipAnimation importerClip = modelImporter.defaultClipAnimations[0];
                AnimationEvent[] evs = new AnimationEvent[methodNames.Count];          
                string[] tempPs;
                for (int i = 0; i < methodNames.Count; i++)
                {
                    tempPs = ps[i].Split('-');
                    AnimationEvent ev = new AnimationEvent
                    {
                        floatParameter = float.Parse(tempPs[0]),
                        intParameter = int.Parse(tempPs[1]),
                        stringParameter =tempPs[2],
                        time = (keys[i]-importerClip.firstFrame)/30f,
                        functionName = methodNames[i]
                    };
                    evs[i]=ev;
                }
                ModelImporterClipAnimation mica = new ModelImporterClipAnimation();
                mica.loopTime = importerClip.loopTime;
                mica.firstFrame = importerClip.firstFrame;
                mica.lastFrame = importerClip.lastFrame;
                mica.name = importerClip.name;
                mica.events = evs;
                modelImporter.clipAnimations = new ModelImporterClipAnimation[] { mica };
                modelImporter.SaveAndReimport();
            }
        }
    }

    /// <summary>
    /// 清除动画事件
    /// </summary>
    [MenuItem("Tools/Animator/ClearEvent")]
    public static void ClearEvent() {
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            string filePath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(filePath) && filePath.EndsWith(".FBX"))
            {
                ModelImporter modelImporter = (ModelImporter)ModelImporter.GetAtPath(filePath);
                if (modelImporter == null || modelImporter.clipAnimations.Length == 0
                    || modelImporter.clipAnimations[0].events.Length == 0)
                    continue;
                ModelImporterClipAnimation importerClip = modelImporter.defaultClipAnimations[0];
                ModelImporterClipAnimation mica = new ModelImporterClipAnimation();
                mica.loopTime = importerClip.loopTime;
                mica.firstFrame = importerClip.firstFrame;
                mica.lastFrame = importerClip.lastFrame;
                mica.name = importerClip.name;
                modelImporter.clipAnimations = new ModelImporterClipAnimation[] { mica };
                modelImporter.SaveAndReimport();
            }
        }
    }

    #endregion

    #region 私有方法
    /// <summary>
    /// 加载技能事件配置文件
    /// </summary>
    private static void LoadFile(string path) {
        if (File.Exists(path))
        {
            StreamReader reader=File.OpenText(path);
            string content=reader.ReadToEnd();
            reader.Dispose();
            skillEvents = ParseSkillEvents(content);
        }else
            Debug.LogWarning("The file path:---"+path+"---don't exists!");
    }

    /// <summary>
    /// 解析技能事件配置文件
    /// </summary>
    private static List<SkillEvent> ParseSkillEvents(string content)
    {
        string[] items=content.Split(',');
        if(items.Length<2)
            return null;
        skillEvents=new List<SkillEvent>();
        SkillEvent skillEvent;
        for (int i = 1; i < items.Length;i++ )
        {
            string[] element=items[i].Split('\t');
            string keys=element[3];
            string[] allKey=keys.Split('/');
            List<int> tempKeys = new List<int>();
            foreach(string s in allKey){
                tempKeys.Add(int.Parse(s));
            }
            string[] parameters = element[6].Split('/');
            Dictionary<int, string> tempParams = new Dictionary<int, string>();
            for (int j = 0; j < parameters.Length;j++ )
            {
                tempParams.Add(j, parameters[j]);
            }

            skillEvent = new SkillEvent
            {
                id=int.Parse(element[0]),
                name=element[1],
                controllerName=element[2],
                keys=tempKeys,
                animName=element[4],
                methodsName = new List<string>(element[5].Split('/')),
                parametes = tempParams
            };
            skillEvents.Add(skillEvent);
        }

        return skillEvents;
    }

    /// <summary>
    /// 绑定behaviour
    /// </summary>
    private static void BindBehaviour(AnimatorState state) { 
        switch(state.name){
            case "idle":
                state.AddStateMachineBehaviour<IdleBehaviour>();
                break;
            case "run":
                state.AddStateMachineBehaviour<RunBehaviour>();
                break;
            case "roll":
                state.AddStateMachineBehaviour<RollBehaviour>();
                break;
            case "dead":
                state.AddStateMachineBehaviour<DeadBehaviour>();
                break;
            case "atk1":
                state.AddStateMachineBehaviour<Atk1Behaviour>();
                break;
            case "atk2":
                state.AddStateMachineBehaviour<Atk2Behaviour>();
                break;
            case "atk3":
                state.AddStateMachineBehaviour<Atk3Behaviour>();
                break;
            case "atk4":
                state.AddStateMachineBehaviour<Atk4Behaviour>();
                break;
            case "skill1":
                state.AddStateMachineBehaviour<Skill1Behaviour>();
                break;
            case "skill2":
                state.AddStateMachineBehaviour<Skill2Behaviour>();
                break;
            case "hit":
                state.AddStateMachineBehaviour<HitBehaviour>();
                break;
        }
    }

    /// <summary>
    /// 给controller增加参数，参数名为motion名，且为bool类型
    /// </summary>
    private static void AddParameter(string path, AnimatorController controller)
    {
        AnimatorStateMachine sm = controller.layers[0].stateMachine;
        //根据动画文件读取它的Motion对象
        Motion motion = AssetDatabase.LoadAssetAtPath(path, typeof(Motion)) as Motion;
        string fileName = FetchFileName(path);       
        AnimatorState state = sm.AddState(fileName);
        //取出动画名子 添加到state里面
        state.motion = motion;
        //根据文件名增加参数
        if (!defaultStateName.Equals(fileName))
            controller.AddParameter(fileName, AnimatorControllerParameterType.Bool);
        else
            sm.defaultState = state;//设置默认state       
    }

    /// <summary>
    /// 根据动画文件路径获取文件名
    /// </summary>
    private static string FetchFileName(string path) {
        return path.Split('@')[1].Split('.')[0];
    }

    /// <summary>
    /// 设置连招的连接
    /// </summary>
    private static void ConnectCombo(Dictionary<int,AnimatorState> combos) {
        int len = combos.Count;
        for (int i = 1; i < len+1;i++ )
        {
            if (i != len) {
                AnimatorStateTransition transitionFrom = combos[i].AddTransition(combos[i+1]);
                transitionFrom.AddCondition(AnimatorConditionMode.If, 0f, combos[i+1].name);
                transitionFrom.duration = 0;
                transitionFrom.exitTime = 0;
            }
            
        }
    }
    #endregion
}
