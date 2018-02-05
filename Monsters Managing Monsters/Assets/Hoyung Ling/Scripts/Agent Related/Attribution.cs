using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribution : Entity {

	public enum Attributes
    {
        None,
        HR,
        IT,
        Janitorial,
        Finance,
        Overseas,
        Marketing,
        Security
    }

    public Attributes myAttribute;

    virtual public void Update()
    {
        if (GameManager.instance.PixelMode) return;
    }
}
