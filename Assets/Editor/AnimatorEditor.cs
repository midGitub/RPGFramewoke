using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorEditor : Editor {

    private static string defaultStateName = "idle";

    [MenuItem("Tools/Animator/Create AnimatorController")]
    public static void CreateAnimatorController() {
        AnimatorController controller = null;
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
           AddStateTransition(path, controller);
       }
       AnimatorStateMachine rootMachine = controller.layers[0].stateMachine;
       AnimatorState defaultState = rootMachine.defaultState;
       ChildAnimatorState[] states = rootMachine.states;
       foreach(ChildAnimatorState state in states){
            if (!rootMachine.defaultState.name.Equals(state.state.name))
            {
                AnimatorStateTransition transition = defaultState.AddTransition(state.state);
                transition.AddCondition(AnimatorConditionMode.If,0f,state.state.name);
                transition.duration = 0;
                transition.exitTime = 0;
            }
            SetBehaviour(state.state);
        }
    }

    //绑定behaviour
    private static void SetBehaviour(AnimatorState state) { 
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

    private static void AddStateTransition(string path, AnimatorController controller)
    {
        AnimatorStateMachine sm = controller.layers[0].stateMachine;
        //根据动画文件读取它的Motion对象
        Motion motion = AssetDatabase.LoadAssetAtPath(path, typeof(Motion)) as Motion;
        string fileName = FetchFileName(path);       
        AnimatorState state = sm.AddState(fileName);
        //取出动画名子 添加到state里面
        state.motion = motion;
        //把ready设置为默认状态,不是默认状态的加个参数
        if (defaultStateName.Equals(fileName))
        {
            AnimatorStateTransition transition = sm.AddAnyStateTransition(state);
            transition.duration = 0;
            transition.exitTime = 1;
            transition.hasExitTime = true;
            sm.defaultState = state;
        }           
        else 
            controller.AddParameter(fileName, AnimatorControllerParameterType.Bool);         
    }

    private static string FetchFileName(string path) {
        return path.Split('@')[1].Split('.')[0];
    }

}
