using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class CBaseLoader
{
    protected uint m_uiKey;
    protected string[] list;

    public virtual uint GetKey() { return m_uiKey; }
    public bool isUpdateAttribute(string[] array)
    {
        if (string.IsNullOrEmpty(array[0])) return false;

        m_uiKey = GetUInt32(array[0]);
        list = array;
        return true;
    }

    protected string GetString(ref string src, string s)
    {
        src = s;
        return s;
    }

    private double GetDouble(string s)
    {
        double value = 0;
        double.TryParse(s, out value);

        return value;
    }

    private float GetSingle(string s)
    {
        float value = 0;
        float.TryParse(s, out value);

        return value;
    }

    protected float GetSingle(ref float src, string s)
    {
        if (src != 0)
        {
            return src;
        }


        if (s.Equals("0"))
        {
            return src;
        }

        float value = 0;
        float.TryParse(s, out value);

        src = value;
        return value;
    }

    private int GetInt32(string s)
    {
        int value = 0;
        Int32.TryParse(s, out value);
        return value;
    }

    protected int GetInt32(ref int src, string s)
    {
        if (src != 0)
        {
            return src;
        }


        if (s.Equals("0"))
        {
            return src;
        }

        int value = 0;
        Int32.TryParse(s, out value);

        src = value;
        return value;
    }

    private uint GetUInt32(string s)
    {
        uint value = 0;
        UInt32.TryParse(s, out value);

        return value;
    }

    protected uint GetUInt32(ref uint src, string s)
    {
        if (src != 0)
        {
            return src;
        }


        if (s.Equals("0"))
        {
            return src;
        }

        uint value = 0;
        UInt32.TryParse(s, out value);

        src = value;
        return value;
    }

    private short GetInt16(string s)
    {
        short value = 0;
        Int16.TryParse(s, out value);

        return value;
    }

    protected short GetInt16(ref short src, string s)
    {
        if (src != 0)
        {
            return src;
        }

        if (s.Equals("0"))
        {
            return src;
        }

        short value = 0;
        Int16.TryParse(s, out value);

        src = value;
        return value;
    }

    private ushort GetUInt16(string s)
    {
        ushort value = 0;
        UInt16.TryParse(s, out value);

        return value;
    }

    protected ushort GetUInt16(ref ushort src, string s)
    {
        if (src != 0)
        {
            return src;
        }

        if (s.Equals("0"))
        {
            return src;
        }

        ushort value = 0;
        UInt16.TryParse(s, out value);

        src = value;
        return value;
    }

    protected bool GetBoolean(ref bool src, string s)
    {
        if (src)
        {
            return src;
        }
        bool result = (s.Equals("0") ? false : true);

        src = result;
        return result;
    }

    private byte GetByte(string s)
    {
        byte value = 0;
        byte.TryParse(s, out value);

        return value;
    }

    protected byte GetByte(ref byte src, string s)
    {
        if (src != 0)
        {
            return src;
        }

        if (s.Equals("0"))
        {
            return src;
        }

        byte value = 0;
        byte.TryParse(s, out value);

        src = value;
        return value;
    }

    //将值转为List;
    protected List<byte> TransStrToByteList(List<byte> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToByteList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            byte tmp = GetByte(strs[i]);
            list.Add(tmp);
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + "TransStrToByteList error,list.count = 0");
        }

        return list;
    }

    protected List<uint> TransRangeStrToUintList(List<uint> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransRangeStrToUintList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        if (strs.Length >= 2)
        {
            uint max = Math.Max(GetUInt32(strs[0]), GetUInt32(strs[1]));
            uint min = Math.Min(GetUInt32(strs[0]), GetUInt32(strs[1]));

            for (uint i = min; i <= max; i++)
            {
                list.Add(i);
            }
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + "TransRangeStrToUintList error,list.count = 0");
        }
        return list;
    }

    //将值转为List;
    protected List<uint> TransStrToUintList(List<uint> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToUintList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            uint tmp = GetUInt32(strs[i]);
            list.Add(tmp);
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + "TransStrToUintList error,list.count = 0");
        }

        return list;
    }

    protected List<int> TransStrToIntList(List<int> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToIntList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            int tmp = GetInt32(strs[i]);
            list.Add(tmp);
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransStrToIntList error,list.count = 0");
        }
        return list;
    }

    protected List<float> TransStrToFloatList(List<float> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToFloatList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            float tmp = GetSingle(strs[i]);
            list.Add(tmp);
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransStrToFloatList error,list.count = 0");
        }
        return list;
    }

    protected List<double> TransStrToDoubleList(List<double> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToFloatList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            double tmp = GetDouble(strs[i]);
            list.Add(tmp);
        }

        if (list.Count == 0)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransStrToFloatList error,list.count = 0");
        }
        return list;
    }

    protected List<string> TransStrToStringList(List<string> list, string arg, params char[] separator)
    {
        if (String.IsNullOrEmpty(arg))
        {
            return list;
        }
        if (null == list)
        {
            GLog.LogError(" TransStrToStringList error !  the 'list' is null.");
            return list;
        }

        if (list.Count != 0)
        {
            return list;
        }

        string[] strs = arg.Split(separator);

        for (int i = 0, len = strs.Length; i < len; ++i)
        {
            list.Add(strs[i]);
        }

        if (list.Count == 0)
        {
            //Debug.LogError(" Key:" + m_uiKey + " TransStrToStringList error,list.count = 0");
        }
        return list;

    }

    protected Vector3 TransToVector3(ref Vector3 src, string arg, params char[] separator)
    {
        if (src != Vector3.zero)
        {
            return src;
        }

        if (arg.Equals("0")) return Vector3.zero;

        string[] strs = arg.Split(separator);
        if (strs.Length != 3)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransToVector3 error,strs.Length != 3:" + arg);
            return Vector3.zero;
        }

        float x = GetSingle(strs[0]);
        float y = GetSingle(strs[1]);
        float z = GetSingle(strs[2]);
        src = new Vector3(x, y, z);

        return src;
    }

    protected Vector4 TransToVector4(ref Vector4 src, string arg, params char[] separator)
    {
        if (src != Vector4.zero)
        {
            return src;
        }

        if (arg == "0") return Vector4.zero;

        string[] strs = arg.Split(separator);
        if (strs.Length != 4)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransToVector4 error,strs.Length != 4:" + arg);
            return Vector3.zero;
        }


        float x = GetSingle(strs[0]);
        float y = GetSingle(strs[1]);
        float z = GetSingle(strs[2]);
        float w = GetSingle(strs[3]);

        src = new Vector4(x, y, z, w);
        return src;
    }
    protected Rect TransToRect(ref Rect src, string arg, params char[] separator)
    {
        if (src.x != 0 || src.y != 0 || src.width != 0 || src.height != 0)
        {
            return src;
        }

        if (arg.Equals("0")) return src;
        string[] strs = arg.Split(separator);
        if (strs.Length != 4)
        {
            //GLog.LogError(" Key:" + m_uiKey + " TransToRect error,strs.Length != 4:" + arg);
            return src;
        }
        src.x = GetSingle(strs[0]);
        src.y = GetSingle(strs[1]);
        src.height = GetSingle(strs[2]);
        src.width = GetSingle(strs[3]);
        return src;
    }

    //将表里的值转为颜色
    protected Color TransStrToColor(ref Color color, string arg, params char[] separator)
    {
        if (color != Color.black)
        {
            return color;
        }

        if (arg.Equals("0")) return Color.white;

        string[] strs = arg.Split(separator);
        if (strs.Length != 3)
        {
            GLog.LogError(" Key:" + m_uiKey + " TransStrToColor error,strs.Length != 3:   " + strs.Length);
        }

        color.r = GetSingle(strs[0]) / 255.0f;
        color.g = GetSingle(strs[1]) / 255.0f;
        color.b = GetSingle(strs[2]) / 255.0f;
        return color;
    }
}

/// <summary>
/// IOS has limitations while using generic dictionary.
/// This is a small fix to let the code working right.
/// </summary>
public class UINTEqualtiyComparer : IEqualityComparer<uint>
{
    public bool Equals(uint a, uint b)
    {
        return a == b;
    }

    public int GetHashCode(uint i)
    {
        return (int)i;
    }
}


public class CStaticDownload<S> : SingletonObject<CStaticDownload<S>> where S : CBaseLoader
{
//#if UNITY_IOS
//    //Since monotouch's AOT compilation can't provide a real JIT mechnism, so the generic types using here are really confusing for the compiler.
//    //The result is, the spesific types derivered from the generic one are not loaded/compiled at AOT stage and a JIT compilation which is not allowed on IOS platform will be requested at runtime.
//    //So we put those possible dummy things here to force the compiler loading/compiling them at AOT stage to avoid this issue:
//    static Dictionary<uint, CActionLoader> m_dict_CActionLoader = new Dictionary<uint, CActionLoader>();
//    static Dictionary<uint, CAllDynamicLoader> m_dict_CAllDynamicLoader = new Dictionary<uint, CAllDynamicLoader>();
//    static Dictionary<uint, CAllSceneLoader> m_dict_CAllSceneLoader = new Dictionary<uint, CAllSceneLoader>();
//    static Dictionary<uint, CAtlasLoader> m_dict_CAtlasLoader = new Dictionary<uint, CAtlasLoader>();
//    static Dictionary<uint, CAttrLoader> m_dict_CAttrLoader = new Dictionary<uint, CAttrLoader>();
//    static Dictionary<uint, CAudioLoader> m_dict_CAudioLoader = new Dictionary<uint, CAudioLoader>();
//    static Dictionary<uint, CAuraLoader> m_dict_CAuraLoader = new Dictionary<uint, CAuraLoader>();
//    static Dictionary<uint, CAwardLoader> m_dict_CAwardLoader = new Dictionary<uint, CAwardLoader>();
//    static Dictionary<uint, CBaseAILoader> m_dict_CBaseAILoader = new Dictionary<uint, CBaseAILoader>();
//    static Dictionary<uint, CBattlePVELoader> m_dict_CBattlePVELoader = new Dictionary<uint, CBattlePVELoader>();
//    static Dictionary<uint, CBuffLoader> m_dict_CBuffLoader = new Dictionary<uint, CBuffLoader>();
//    static Dictionary<uint, CCameraLoader> m_dict_CCameraLoader = new Dictionary<uint, CCameraLoader>();
//    static Dictionary<uint, CDropItemLoader> m_dict_CDropItemLoader = new Dictionary<uint, CDropItemLoader>();
//    static Dictionary<uint, CDropObjectLoader> m_dict_CDropObjectLoader = new Dictionary<uint, CDropObjectLoader>();
//    //static Dictionary<uint, CDynamicLoader> m_dict_CDynamicLoader = new Dictionary<uint, CDynamicLoader>();
//    static Dictionary<uint, CEquipLoader> m_dict_CEquipLoader = new Dictionary<uint, CEquipLoader>();
//    static Dictionary<uint, CExtraAILoader> m_dict_CExtraAILoader = new Dictionary<uint, CExtraAILoader>();
//    static Dictionary<uint, CGUITextLoader> m_dict_CGUITextLoader = new Dictionary<uint, CGUITextLoader>();
//    static Dictionary<uint, CHomeNPCLoader> m_dict_CHomeNPCLoader = new Dictionary<uint, CHomeNPCLoader>();
//    static Dictionary<uint, CItemBaseLoader> m_dict_CItemBaseLoader = new Dictionary<uint, CItemBaseLoader>();
//    static Dictionary<uint, CItemLoader> m_dict_CItemLoader = new Dictionary<uint, CItemLoader>();
//    //static Dictionary<uint, CLabelLoader> m_dict_CLabelLoader = new Dictionary<uint, CLabelLoader>();
//    static Dictionary<uint, CLevelUpLoader> m_dict_CLevelUpLoader = new Dictionary<uint, CLevelUpLoader>();
//    static Dictionary<uint, CMonsterLoader> m_dict_CMonsterLoader = new Dictionary<uint, CMonsterLoader>();
//    static Dictionary<uint, CMountsLoader> m_dict_CMountsLoader = new Dictionary<uint, CMountsLoader>();
//    static Dictionary<uint, CNameLoader> m_dict_CNameLoader = new Dictionary<uint, CNameLoader>();
//    static Dictionary<uint, CNodeLoader> m_dict_CNodeLoader = new Dictionary<uint, CNodeLoader>();
//    static Dictionary<uint, CNpcBaseLoader> m_dict_CNpcBaseLoader = new Dictionary<uint, CNpcBaseLoader>();
//    static Dictionary<uint, CPanelLoader> m_dict_CPanelLoader = new Dictionary<uint, CPanelLoader>();
//    static Dictionary<uint, CPartenrLoader> m_dict_CPartenrLoader = new Dictionary<uint, CPartenrLoader>();
//    static Dictionary<uint, CParticleLoader> m_dict_CParticleLoader = new Dictionary<uint, CParticleLoader>();
//    static Dictionary<uint, CPetLoader> m_dict_CPetLoader = new Dictionary<uint, CPetLoader>();
//    static Dictionary<uint, CPetSkillLoader> m_dict_CPetSkillLoader = new Dictionary<uint, CPetSkillLoader>();
//    static Dictionary<uint, CPlayerConfig> m_dict_CPlayerConfig = new Dictionary<uint, CPlayerConfig>();
//    static Dictionary<uint, CPlayerLoader> m_dict_CPlayerLoader = new Dictionary<uint, CPlayerLoader>();
//    static Dictionary<uint, CQuestLoader> m_dict_CQuestLoader = new Dictionary<uint, CQuestLoader>();
//    static Dictionary<uint, CReliceLoader> m_dict_CReliceLoader = new Dictionary<uint, CReliceLoader>();
//    static Dictionary<uint, CSceneConfigLoader> m_dict_CSceneConfigLoader = new Dictionary<uint, CSceneConfigLoader>();
//    static Dictionary<uint, CSceneInfoLoader> m_dict_CSceneInfoLoader = new Dictionary<uint, CSceneInfoLoader>();
//    static Dictionary<uint, CSceneLoader> m_dict_CSceneLoader = new Dictionary<uint, CSceneLoader>();
//    static Dictionary<uint, CSceneStoryLoader> m_dict_CSceneStoryLoader = new Dictionary<uint, CSceneStoryLoader>();
//    static Dictionary<uint, CScriptLoader> m_dict_CScriptLoader = new Dictionary<uint, CScriptLoader>();
//    static Dictionary<uint, CShopLoader> m_dict_CShopLoader = new Dictionary<uint, CShopLoader>();
//    static Dictionary<uint, CSkillEffectLoader> m_dict_CSkillEffectLoader = new Dictionary<uint, CSkillEffectLoader>();
//    static Dictionary<uint, CSkillLoader> m_dict_CSkillLoader = new Dictionary<uint, CSkillLoader>();
//    static Dictionary<uint, CSkillUpLoader> m_dict_CSkillUpLoader = new Dictionary<uint, CSkillUpLoader>();
//    static Dictionary<uint, CTalkContentLoader> m_dict_CTalkContentLoader = new Dictionary<uint, CTalkContentLoader>();
//    static Dictionary<uint, CTechLoader> m_dict_CTechLoader = new Dictionary<uint, CTechLoader>();
//    static Dictionary<uint, CTipsLoader> m_dict_CTipsLoader = new Dictionary<uint, CTipsLoader>();
//    static Dictionary<uint, CTriggerLoader> m_dict_CTriggerLoader = new Dictionary<uint, CTriggerLoader>();
//#endif

    private Dictionary<uint, S> m_dict = new Dictionary<uint, S>(new UINTEqualtiyComparer());

    public void LoadCompleted(string content)
    {
        m_dict.Clear();
        string[] arrayList = content.Split("\r\n".ToCharArray());
        string s;
        string[] field;
        string head;
        int count = arrayList.Length;
        for (int i = 0; i < count; ++i)
        {
            if (0 == i)//数据表说明
            {
                continue;
            }
            s = arrayList[i];
            if (s.Length <= 0) continue;
            field = s.Split('\t');
            S info = (S)Activator.CreateInstance(typeof(S));
#if UNITY_EDITOR
            head = field[0];
            if (head.Equals("") || head.Equals(" "))
            {
                GLog.LogError(info.GetType().ToString() + " line:" + i + "  is empty");
                continue;
            }
#endif
            if (info.isUpdateAttribute(field))
            {
                uint uiKey = info.GetKey();
                if (m_dict.ContainsKey(uiKey))
                {
                    GLog.LogError(info.GetType().ToString() + " has the same key:" + info.GetKey());
                    continue;
                }
                m_dict.Add(uiKey, info);
            }
        }
    }

    public S GetStaticInfo(uint uiKey)
    {
        S info = null;
        if (0 == uiKey)
        {
            return info;
        }

        m_dict.TryGetValue(uiKey, out info);

        if (null == info)
        {
            GLog.Log("can't find key " + uiKey + " in " + GetType().ToString());
        }
        return info;
    }

    public Dictionary<uint, S> GetDict()
    {
        return m_dict;
    }

    public void Clear()
    {
        m_dict.Clear();
    }

}


