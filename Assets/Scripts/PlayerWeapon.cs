using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWeapon : Weapon {
    
    public TMPro.TMP_Text uiAmmoCount;
    public PlayerScript player;
    
    protected override void UpdatePlayerUI() => uiAmmoCount.SetText(roundsRemaining.ToString());

    protected virtual void RegisterFire() => player.shotsFired++;
}

