using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems; // using deseleccionar

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]

public class OptionsButton : MonoBehaviour
{
    [SerializeField] private Text m_text = null;
    private Button m_button = null;
    private Color m_originalColor = Color.black;
    private Image m_image = null;

    public Q_options Option { get; set;  }

    public void Awake()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_text = transform.GetChild(0).GetComponent<Text>();
        
        m_originalColor = m_image.color;
    }

    public void Construct(Q_options option, Action<OptionsButton> callback)
    {
        m_text.text = option.text;

        m_button.onClick.RemoveAllListeners();
        m_button.enabled = true;
        m_image.color = m_originalColor;

        Option = option;

        m_button.onClick.AddListener(delegate
        {
            callback(this);
            EventSystem.current.SetSelectedGameObject(null); // Deselecciona el botón después de hacer clic en él
        });
    }
    /*
    public void SetColor(Color c)
    {
        m_button.enabled = false;
        m_image.color = c;
    }
    */
}
