using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction
{
    public abstract AIState Execute(List<string> tableCards, List<string> aiHand, Player player);
}
