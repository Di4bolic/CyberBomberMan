using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterRange : Power
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void DoStuff(Player player)
    {
        player.range += 1;
        player.rangeText.text = "Range : " + player.range;
    }
}
