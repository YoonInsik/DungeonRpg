using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CookingItem : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    public int ATK;
    public int DEF;
    public float Fullness;
    public float Speed;
    public float ATKSpeed;
    public float ATKRange;
    public float CooldownReduction;
    public float ATKduration;
    public float Greed;
    public float Delicacy;
    public float Wisdom;
    public float Temptation;
    public float buffDuration;
    //요리 레시피
    [Serializable]
    public struct Ingredient
    {
        public MeatItem ingredient;
        public int amount;
    }

    [SerializeField] public List<Ingredient> recipe;


    //요리 아이템 효과 적용
    public IEnumerator CookingEffect(Player player)
    {
        AddEffect(player);
        if (buffDuration != 0)
        {
            yield return new WaitForSeconds(buffDuration);
            EndEffect(player);
            Debug.Log(name + " " + buffDuration + "초 Buff끝");
        }
        else
        {
            Debug.Log(name + "은 버프가 아님");
        }
    }

    //적용될 아이템 효과
    public abstract void AddEffect(Player player);
    //버프지속시간이 있을 시 적용된 효과 제거
    public virtual void EndEffect(Player player) { }


    //체력 회복
    public void HPRecovery(Player player, bool RecoveryFull = false)
    {
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1 : (1 + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        if (player.HP + HP >= player.MaxHP || RecoveryFull == true)
        {
            player.HP = player.MaxHP;
        }
        else
        {
            player.HP += (int)(HP * DelicacyRate);
        }
    }

    //최대 체력 증가
    public void IncreaseHP(Player player, bool isPositive = true)
    {
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1 : (1 + (float)player.PlayerStatLevel.DelicacyLevel / 10);
        Debug.Log(player.PlayerStatLevel.DelicacyLevel +  DelicacyRate);
        int change = isPositive ? MaxHP : -MaxHP;
        player.MaxHP += (int)(change * DelicacyRate);
    }

    //공격력 증가
    public void IncreaseATK(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel;

        int change = isPositive ? ATK : -ATK;
        player.ATK += change + DelicacyRate;

        player.PlayerStatLevel.ATKLevel++;
    }

    //방어력 증가
    public void IncreaseDEF(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DEFLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel;

        int change = isPositive ? DEF : -DEF;
        player.DEF += change + DelicacyRate;

        player.PlayerStatLevel.DEFLevel++;
    }

    //포만감 감소량 증가
    public void IncreaseFullnessDecrease(Player player, bool isPositive = true)
    {
        int change = isPositive ? 1 : - 1;
        player.fullnessDecreaseAmount += change;
    }

    //이동속도 증가
    public void IncreaseSpeed(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.SpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        //미식에 따른 수치 적용
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        float change = isPositive ? Speed : -Speed;
        player.speed += change*DelicacyRate;

        player.PlayerStatLevel.SpeedLevel++;
    }

    //투사체 이동,회전속도
    public void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKSpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.ATKSpeedLevel++;
    }

    //공격범위,투사체 크기 증가
    public void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKRangeLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.ATKRangeLevel++;
    }

    //쿨타임 감소
    public void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.CooldownReductionLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.CooldownReductionLevel++;
    }

    //공격이 발동할때 그 공격이 지속되는 시간 증가
    public void IncreaseATKDuration(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKdurationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.ATKdurationLevel++;
    }

    //스테이지 클리어 후 얻는 재료양 증가
    public void IncreaseGreed(Player player , bool isPositive = true)
    {
        if (player.PlayerStatLevel.GreedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.GreedLevel++;
    }

    //요리섭취시 레벨당 오르는 능력치 증가
    public void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DelicacyLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        player.PlayerStatLevel.DelicacyLevel++;
    }

    //획득하는 경험치 획득량 증가
    public void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.WisdomLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        player.PlayerStatLevel.WisdomLevel++;
    }

    //전투중 경험치 보석, 아이템을 획득하는 범위가 증가
    public void IncreaseTemptation(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.TemptationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        float change = isPositive ? Temptation / 2 : -Temptation / 2;
        player.GetComponent<ItemTemptation>().range += change * DelicacyRate;

        player.PlayerStatLevel.TemptationLevel++;
    }

    public void MaxLevelCheck(Player player)
    {
        
    }
}
