using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Dialog Config", menuName = "Configs/Dialog")]
public class DialogConfig : ScriptableObject
{
    [SerializeField] private string previewText;
    [ResizableTextArea]
    [SerializeField][TextArea] private string dialogText;
    [Expandable]
    [SerializeField] private DialogAction BeginDialogAction;
    [Expandable]
    [SerializeField] private List<DialogConfig> choices;

    public string GetDialogText() { return dialogText; }
    public string GetPreviewText() { return previewText; }

    public List<DialogConfig> GetChoices() {  return choices; }

    public DialogAction GetBeginDialogAction() { return BeginDialogAction; }
}
