using _02_DBManager;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CDBManager : MonoBehaviour
{
    public GameObject effectPassPrefab;
    public GameObject effectFailPrefab;
    public TMP_InputField inputId;
    public TMP_InputField inputPass;

    public void HasMember()
    {
        string id = inputId.text;
        string pass = inputPass.text;

        MemberDao mDao = new MemberDao();
        MemberVo vo = mDao.GetMemberData(id, pass);

        GameObject effectObj;
        if (String.IsNullOrEmpty(vo.MEMBER_ID))
            effectObj = Instantiate(effectFailPrefab, transform);
        else
        {
            Debug.Log(vo.ToString());
            effectObj = Instantiate(effectPassPrefab, transform);
        }

        Destroy(effectObj, 3f);
    }
}
