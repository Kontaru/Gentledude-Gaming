using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public enum Entities
    {
        None,
        NPC,
        Player,
        Hero
    }

    public Entities EntityType;
}
