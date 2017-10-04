using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRespondsToDeath
{
    void OnDeath(DamageContext context);
}