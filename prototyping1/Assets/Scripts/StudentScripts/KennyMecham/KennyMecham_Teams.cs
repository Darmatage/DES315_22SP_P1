using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KennyMecham_Teams : MonoBehaviour
{
    public List<string> teams;

    public bool DoTeamsOverlap(KennyMecham_Teams otherTeams)
    {
        foreach(string team in otherTeams.teams)
        {
            if(teams.Contains(team))
            {
                return true;
            }
        }

        return false;
    }
}
