using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWeapon : Weapon {
    
    public TMPro.TMP_Text uiAmmoCount;
    
    protected override void UpdatePlayerUI() {
        uiAmmoCount.SetText(roundsRemaining.ToString());
    }
}

