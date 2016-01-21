using UnityEngine;
using System.Collections;

public class HeroFBX : MonoBehaviour {
    private GameObject mParent;

    public void SetParent(GameObject parent)
    {
        mParent = parent;
    }

    public void OnAttackEnd()
    {
        if (mParent != null)
            mParent.SendMessage("OnAttackEnd");
    }

    public void OnSkillEnd() {
        if (mParent != null)
            mParent.SendMessage("OnSkillEnd");
    }
}
