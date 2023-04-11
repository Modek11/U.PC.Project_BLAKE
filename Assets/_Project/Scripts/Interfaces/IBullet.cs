using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public void SetupBullet(float xSpread, GameObject instigator);
}
