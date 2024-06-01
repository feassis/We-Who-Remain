using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailEntry : MonoBehaviour
{
    [SerializeField] private GameObject newMailWarning;
    [SerializeField] private TextMeshProUGUI emailPreview;
    [SerializeField] private Button emailButtom; 

    private Mail email;

    private void Awake()
    {
        emailButtom.onClick.AddListener(OnEmailButtom);
    }

    private void OnEmailButtom()
    {
        GameMaster.Instance.MarkEmailAsRead(email);

        GameMaster.Instance.LoadDialog(email.email, email.Character);
    }

    public void Setup(Mail mail)
    {
        email = mail;
        emailPreview.text = $"{mail.Character.GetCharacter().Name} sent: " + email.email.GetPreviewText();
        newMailWarning.SetActive(!email.wasRead);
    }
}
