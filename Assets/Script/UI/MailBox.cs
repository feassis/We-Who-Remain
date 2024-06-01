using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBox : MonoBehaviour
{
    [SerializeField] private RectTransform mailEntryHolder;
    [SerializeField] private MailEntry mailEntryPrefab;

    private List<MailEntry> mailEntries = new List<MailEntry>();

    public void LoadMails(List<Mail> emails)
    {
        foreach (var mail in emails)
        {
            MailEntry entry = Instantiate<MailEntry>(mailEntryPrefab, mailEntryHolder);
            entry.Setup(mail);
            mailEntries.Add(entry);
        }
    }

    public void UnloadMails()
    {
        foreach (var mail in mailEntries)
        {
            Destroy(mail.gameObject);
        }   

        mailEntries.Clear();
    }
}
