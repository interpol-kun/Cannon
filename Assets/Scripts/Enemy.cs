using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Default interface for all the enemies in the game
interface IEnemy
{
    int Bounty { get; set; }
    float Speed { get; set; }

    int Health { get;}

    void Death(bool isKilled, float delay);

    //We can use this method to actually heal enemies with the negative damage value
    void TakeDamage(int damage);
}
