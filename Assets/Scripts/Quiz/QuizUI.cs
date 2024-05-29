using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_question = null;
    [SerializeField] private List<OptionsButton> m_buttonlist = null;

    public void Construct(Q_question q, Action<OptionsButton> callback)
    {
        m_question.text = q.text;

        for (int n= 0; n< m_buttonlist.Count; n++)
        {
            m_buttonlist[n].Construct(q.options[n], callback);
        }
    }
}
