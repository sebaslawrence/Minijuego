using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q_DataBase : MonoBehaviour
{
    [SerializeField] private List<Q_question> m_question_list = null;
    private List<Q_question> m_backup;

    private void Awake()
    {
            m_backup = m_question_list.ToList();
    }

    public Q_question GetRandom(bool remove = true)
    {
        if (m_question_list.Count == 0)
            RestoreBackup();

        int index = Random.Range(0, m_question_list.Count);

        if (!remove)
            return m_question_list[index];

        Q_question q = m_question_list[index];
        m_question_list.RemoveAt(index);

        return q;
    }

    private void RestoreBackup()
    {
        m_question_list = m_backup.ToList();
    }
}
