using System;
using UnityEngine;

public class ClearMind : Effect
{
    private bool isMoving = false;
    private float duration = 5f;
    private float healDuration = 1f;
    
    private void Awake()
    {
        ActivateEffect();
    }

    public override void ActivateEffect()
    {
        GlobalEvent<bool>.Subscribe("PlayerMoveStatusChange", OnPlayerMoveStatusChange);
    }
    
    private void OnPlayerMoveStatusChange(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Paused) return;

        if (isMoving)
        {
            duration = 5f;
            return;
        }
        
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            duration = 0;
            
            healDuration -= Time.deltaTime;
            if (healDuration <= 0)
            {
                healDuration = 1f;
                CacheDataManager.Instance.player.HealPercentage(0.5f);
            }
        }
    }

    public override void DeactivateEffect()
    {
        GlobalEvent<bool>.Unsubscribe("PlayerMoveStatusChange", OnPlayerMoveStatusChange);
    }
}
