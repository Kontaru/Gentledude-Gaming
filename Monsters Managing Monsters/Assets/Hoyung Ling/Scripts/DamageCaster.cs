using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : Attribution {

    //Does this belong to the hero? 
    //If it does, only damage NPCs
    //If not, they belong to the NPC and thus can only attack the hero
    public bool belongsToHero = false;

    //Triggers when hitting another collider
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<Entity>() != null)
        {
            if (coll.gameObject.GetComponent<Entity>().EntityType == Entity.Entities.Hero && !belongsToHero)
            {
                //On a successful hit (depending on the boolean), check attributes and deal damage
                AttributeCheck(coll);
            }
            else if (coll.gameObject.GetComponent<Entity>().EntityType == Entity.Entities.Monster && belongsToHero)
            {
                //On a successful hit (depending on the boolean), check attributes and deal damage
                AttributeCheck(coll);
            }
        }
    }

    void Start()
    {
        //Audio for attack, and destroy game object after a few seconds (to simulate a swing)
        AudioManager.instance.Play("Attack 1");
        Destroy(gameObject, 0.2f);
    }

    #region --- Damage ---

    //Checks the attribute of the weapon, and the attribute of what's been collided. 
    //If they are in favour of the weapon, deal double damage. Otherwise deal 1
    void AttributeCheck(Collider coll)
    {
        if (myAttribute == Attributes.Succubus)
        {
            if (coll.GetComponent<Attribution>().myAttribute == Attribution.Attributes.Goblin) DealDamage(coll, 2);
            else DealDamage(coll, 1);
        }
        else if (myAttribute == Attributes.Ogre)
        {
            if (coll.GetComponent<Attribution>().myAttribute == Attribution.Attributes.Succubus) DealDamage(coll, 2);
            else DealDamage(coll, 1);
        }
        else if (myAttribute == Attributes.Goblin)
        {
            if (coll.GetComponent<Attribution>().myAttribute == Attribution.Attributes.Ogre) DealDamage(coll, 2);
            else DealDamage(coll, 1);
        }
    }

    //Send message "TakeDamage" to the collider
    void DealDamage(Collider coll, float damage)
    {
        coll.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }

    #endregion
}
