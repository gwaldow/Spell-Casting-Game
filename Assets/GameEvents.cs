using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayAudioArgs : EventArgs
{
    public string name;
}

public class DisplayMessageArgs : EventArgs
{
    public string message;
    public int displayTime;
}

public class AquirePagesArgs : EventArgs
{
    public List<Texture2D> pages;
}


public static class GameEvents
{
    public static event Action AquireBook;
    public static event Action AquireChalk;
    public static event Action PlayerDamage;
    public static event EventHandler<PlayAudioArgs> PlayAudio;
    public static event EventHandler<DisplayMessageArgs> DisplayMessage;
    public static event EventHandler<AquirePagesArgs> AquirePages;


    public static void InvokeAquireBook()
    {
        AquireBook();
    }

    public static void InvokeAquirePages(List<Texture2D> pagesAquired)
    {
        Debug.Log(pagesAquired);
        Debug.Log(pagesAquired.Count);
        AquirePages(null, new AquirePagesArgs { pages = pagesAquired});
    }

    public static void InvokeAquireChalk()
    {
        AquireChalk();
    }

    public static void InvokePlayerDamage()
    {
        PlayerDamage();
    }
    
    public static void InvokePlayAudio(string paramName)
    {
        PlayAudio(null, new PlayAudioArgs { name = paramName});
    }
    
    public static void InvokeDisplayMessage(string message, int time)
    {
        DisplayMessage(null, new DisplayMessageArgs { message = message , displayTime = time});
    }
}

