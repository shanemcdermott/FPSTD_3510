using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains enums and structs used by damage systems.
/// </summary>

/// <summary>
/// Types of Damage that can be dealt.
/// </summary>
public enum DamageType
{
    Physical,
    Electric,
    Fire,
    Cold
}


/// <summary>
/// When an object attempts to deal damage, DamageContext provides the context necessary for the recipient to make any appropriate adjustments/reactions.
/// </summary>
public struct DamageContext
{
    /// <summary> The GameObject which is responsible for the damage being dealt. </summary>
    /// <value>
    /// The source property provides a reference to the DamageContext's instigator.
    /// </value>
    public GameObject source;
    
    /// <summary>The current amount of damage that is being dealt.</summary>
    /// <value>The amount property may be adjusted as it passes through various defensive systems before it actually removes health.</value>
    public int amount;
    /// <summary>
    /// The type of damage that is being dealt. This may affect the amount due to weaknesses/resistances/other modifiers.
    /// </summary>
    public DamageType type;
    /// <summary>
    /// The center location of the damage being dealt. Can be used for weak spot multipliers.
    /// </summary>
    public Vector3 hitLocation;

    /// <summary>
    /// Constructor for physical damage with a location of 0,0,0
    /// </summary>
    /// <param name="source">The GameObject which is responsible for the damage being dealt.</param>
    /// <param name="amount">The initial amount of damage being dealt.</param>
    public DamageContext(GameObject source, int amount)
    {
        this.source = source;
        this.amount = amount;
        this.type = DamageType.Physical;
        this.hitLocation = new Vector3();
    }

    /// <summary>
    /// Constructor for physical damage with a hitLocation.
    /// </summary>
    /// <param name="source">The GameObject which is responsible for the damage being dealt.</param>
    /// <param name="amount">The initial amount of damage being dealt.</param>
    /// <param name="hitLocation">The central location of the damage being dealt.</param>
    public DamageContext(GameObject source, int amount, Vector3 hitLocation)
    {
        this.source = source;
        this.amount = amount;
        this.type = DamageType.Physical;
        this.hitLocation = hitLocation;
    }

    /// <summary>
    /// Constructor for damage context with a specific type of damage.
    /// </summary>
    /// <param name="source">The GameObject which is responsible for the damage being dealt.</param>
    /// <param name="amount">The initial amount of damage being dealt.</param>
    /// <param name="hitLocation">The central location of the damage being dealt.</param>
    /// <param name="type">Type of damage that is being dealt.</param>
    public DamageContext(GameObject source, int amount, Vector3 hitLocation, DamageType type)
    {
        this.source = source;
        this.amount = amount;
        this.type = type;
        this.hitLocation = hitLocation;
    }
}