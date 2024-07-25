using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void Purchase(float price, string itemBought)
    {
        if (GameManager.instance.coinTotal - price >= 0)
        {
            GameManager.instance.LoseCoin(price);
            GameManager.instance.userCollectibles.Add(itemBought);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
