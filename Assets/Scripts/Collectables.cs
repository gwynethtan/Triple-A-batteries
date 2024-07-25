using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

// Base class for all collectibles
public abstract class Collectible : MonoBehaviour
{
    public abstract void Collect();
}

// Child class for Poop
public class Poop : Collectible
{
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.LoseAura(Random.Range(5.0f, 15.0f));
        GameManager.instance.Notify("You Stepped on Poop!", "Your Aura has been deducted.")
        GameManager.AddCollectible("Poop");
        Destroy(gameObject);
    }
}

// Child class for Litter
public class Litter : Collectible
{
    public override void Collect()
    {
        GameManager.AddCollectible("Litter");
        Destroy(gameObject);
    }
}

// Child class for Coins
public class Coins : Collectible
{
    public override void Collect()
    {
        GameManager.AddCollectible("Coins");
        Destroy(gameObject);
    }
}
