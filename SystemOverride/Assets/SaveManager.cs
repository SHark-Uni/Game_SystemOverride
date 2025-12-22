using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager
{
    // À§Ä¡
    public static void SavePlayer(Vector3 position, int hp)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.SetInt("PlayerHP", hp);

        PlayerPrefs.SetInt("HasSave", 1);
        PlayerPrefs.Save();
    }

    public static bool HasSave()
    {
        return PlayerPrefs.GetInt("HasSave", 0) == 1;
    }

    public static Vector3 LoadPlayerPosition()
    {
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        return new Vector3(x, y, 0);
    }

    public static int LoadPlayerHP()
    {
        return PlayerPrefs.GetInt("PlayerHP");
    }
}
