using UnityEngine;
using System.Collections;

public class CSkillLoader : CBaseLoader
{
    //技能名称
    private string m_skillName;
    public string GetSkillName()
    {
        return GetString(ref m_skillName, list[1]);
    }

    //技能CD
    private float m_cd;
    public float GetCD()
    {
        return GetSingle(ref m_cd, list[15]);
    }

    //持续时间
    private float m_lastTime;
    public float GetlastTime()
    {
        return GetSingle(ref m_lastTime, list[18]);
    }
}
