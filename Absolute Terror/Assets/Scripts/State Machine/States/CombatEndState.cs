using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CombatEndState : State
{
    
    public override void Enter()
    {
        base.Enter();
        stMachine.combatEndPanel.MoveTo("Show");
        
        Alliance victorAlliance = MapLoader.instance.alliances.Find(alliance => alliance.active);
        stMachine.combatEndText.SetText("Faction " + victorAlliance.factions[0] + " Wins!");
    }

    
}
