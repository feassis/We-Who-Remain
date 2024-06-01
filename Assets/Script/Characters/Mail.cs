using System;

[Serializable]
public class Mail
{
    public bool wasRead;
    public CharacterInGame Character;
    public DialogConfig email;

    public Mail(DialogConfig email, CharacterInGame character)
    {
        this.email = email;
        Character = character;
    }
}
