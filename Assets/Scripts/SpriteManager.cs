using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    private static SpriteManager instance;
    public static SpriteManager Instance
    {
        get
        {
            if(instance == null)
                instance= FindObjectOfType<SpriteManager>();
            return instance;
         }
    }
    public Sprite[] sprites;
    Dictionary<string, Sprite> spritesDic=new Dictionary<string, Sprite>();
    void Awake()
    {
        sprites = Resources.LoadAll<Sprite>("Images");
        for (int i = 0; i < sprites.Length; i++)
        {
            spritesDic.Add(sprites[i].name, sprites[i]);
        }
        
    }
    public Sprite GettingSprite(string name)
    {
        
        if(spritesDic.ContainsKey(name))
            return spritesDic[name];
        return null;
    }

   
}
