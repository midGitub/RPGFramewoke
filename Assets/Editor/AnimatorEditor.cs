using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;
using System;

public class AnimatorEditor : Editor {

    private static string defaultStateName = "idle";
    private static Dictionary<int, AnimatorState> combos;

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

    private static string dirName = "StaticDatas";
    private static string fileName = "skillEvent.txt";
    private static List<SkillEvent> skillEvents;

    [MenuItem("Tools/Animator/BindEvent")]
    public static void BindEvent()
    {
        //AnimationUtility.SetAnimationEvents

        skillEvents = null;
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
            if (obj is AnimatorController) {
                if (skillEvents == null)
                    LoadFile(Application.dataPath + "/" + dirName+"/"+fileName);
                AnimatorController  controller= obj as AnimatorController;
                List<SkillEvent> result=skillEvents.FindAll(x => x.controllerName.Equals(controller.name));
                if (result == null || result.Count == 0)
                    continue;
                foreach(SkillEvent skillEvent in result){
                    AnimationClip[] clips=controller.animationClips;
                    foreach(AnimationClip clip in clips){
                        if(clip.name.Equals(skillEvent.animName)){
                            List<string> methods=skillEvent.methodsName;
                            Dictionary<int,string> ps=skillEvent.parametes;
                            AnimationEvent[] events=new AnimationEvent[methods.Count];
                            string[] tempPs;
                            for (int i = 0; i < methods.Count;i++ )
                            {
                                tempPs = ps[i].Split('-');
                                AnimationEvent ev = new AnimationEvent
                                {
                                    functionName=methods[i],
                                    floatParameter = float.Parse(tempPs[0]),
                                    intParameter=int.Parse(tempPs[1]),
                                    stringParameter=tempPs[2],          
                                    
                                };
                                //clip.AddEvent(ev);
                                events[i]=ev;
                            }
                            AnimationUtility.SetAnimationEvents(clip,events);
                        }
                    }
                }
            }
        }
    }

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

    struct SkillEvent {
        public int id;
        public string name;
        public string controllerName;
        public List<int> keys;
        public string animName;
        public List<string> methodsName;
        public Dictionary<int, string> parametes;
    }

    //绑定behaviour
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

    //给controller增加参数，参数名为motion名，且为bool类型
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

    //根据动画文件路径获取文件名
    private static string FetchFileName(string path) {
        return path.Split('@')[1].Split('.')[0];
    }

    //设置连招的连接
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

}
