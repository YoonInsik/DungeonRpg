using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CookingItem : MonoBehaviour
{
    public int MaxHP;
    public int HP;
    public int ATK;
    public int DEF;
    public float Fullness;
    public int Speed;
    public int ATKSpeed;
    public int ATKRange;
    public int CooldownReduction;
    //public float ATKduration;
    public int Greed;
    public int Delicacy;
    public int Wisdom;
    public int Temptation;
    public float buffDuration;

    //SHS 정보
    [Multiline(4)]
    public string info_txt;

    //요리 레시피
    [Serializable]
    public struct Ingredient
    {
        public MeatItem ingredient;
        public int amount;
    }

    [SerializeField] public List<Ingredient> recipe;


    //요리 아이템 효과 적용
    public virtual IEnumerator CookingEffect(Player player)
    {
        float elapsedtime = 0f;
        AddEffect(player);
        if (buffDuration != 0)
        {
            while (elapsedtime < buffDuration)
            {
                if (!player.pause) elapsedtime++;
                yield return new WaitForSeconds(1f);
            }
            EndEffect(player);
        }
        else
        {
            Debug.Log(name + "은 버프가 아님");
        }
    }

    //적용될 아이템 효과
    protected abstract void AddEffect(Player player);
    //버프지속시간이 있을 시 적용된 효과 제거
    protected virtual void EndEffect(Player player) { }

    

    //체력 회복
    protected virtual void HPRecovery(Player player, bool RecoveryFull = false)
    {
        Dictionary<string, int> dict = player.StatLevels;

        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1 : (1 + (float)dict["DelicacyLevel"] / 10);

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
    protected virtual void IncreaseHP(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;

        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1 : (1 + (float)dict["DelicacyLevel"] / 10);

        int change = isPositive ? MaxHP : -MaxHP;
        player.MaxHP += (int)(change * DelicacyRate);
    }

    //공격력 증가
    protected virtual void IncreaseATK(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKLevel"] >= dict["StatMaxLevel"]) return;

        int DelicacyRate = dict["DelicacyLevel"]*10;
       
        // 더할 레벨
        int Level = isPositive ? ATK : -ATK;
        int change = ApplyAtkDefSpeed(dict, "ATKLevel", Level);

        player.ATK += change * DelicacyRate;
    }

    //방어력 증가
    protected virtual void IncreaseDEF(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["DEFLevel"] >= dict["StatMaxLevel"]) return;

        int DelicacyRate = dict["DelicacyLevel"]*10;

        // 더할 레벨
        int Level = isPositive ? DEF : -DEF;
        int change = ApplyAtkDefSpeed(dict, "DEFLevel", Level);

        player.DEF += change * DelicacyRate;
    }

    //포만감 감소량 증가
    protected virtual void IncreaseFullnessDecrease(Player player, bool isPositive = true)
    {
        int change = isPositive ? 1 : - 1;
        player.fullnessDecreaseAmount += change;
    }

    //이동속도 증가
    protected virtual void IncreaseSpeed(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["SpeedLevel"] >= dict["StatMaxLevel"]) return;

        //DelicacyLevel에 따른 수치 적용
        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1f : (1 + (float)dict["DelicacyLevel"] / 10);

        // 더할 레벨
        int Level = isPositive ? Speed : -Speed;
        int change = ApplyAtkDefSpeed(dict, "SpeedLevel", Level);
     
        player.speed += change * DelicacyRate;
    }

    //투사체 이동,회전속도
    protected virtual void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKSpeedLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? ATKSpeed : ATKSpeed;

        ApplyOtherStat(dict, "ATKSpeedLevel", Level);
    }

    //공격범위,투사체 크기 증가
    protected virtual void IncreaseATKRange(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKRangeLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? ATKRange : -ATKRange;

        ApplyOtherStat(dict, "ATKRangeLevel", Level);
        
    }

    //쿨타임 감소
    protected virtual void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["CooldownReductionLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? CooldownReduction : -CooldownReduction ;

        ApplyOtherStat(dict, "CooldownReductionLevel", Level);
    }

    //공격이 발동할때 그 공격이 지속되는 시간 증가
    //protected virtual void IncreaseATKDuration(Player player, bool isPositive = true)
    //{
    //    if (player.PlayerStatLevel.ATKdurationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

    //    int Level = isPositive ? 1 : -1;
    //    player.PlayerStatLevel.ATKdurationLevel += Level;
    //}

    //스테이지 클리어 후 얻는 재료양 증가
    protected virtual void IncreaseGreed(Player player , bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["GreedLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Greed : -Greed;


        ApplyOtherStat(dict, "GreedLevel", Level);
    }

    //요리섭취시 레벨당 오르는 능력치 증가
    protected virtual void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["DelicacyLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Delicacy : -Delicacy;

        ApplyOtherStat(dict, "DelicacyLevel", Level);
    }
        
    //획득하는 경험치 획득량 증가
    protected virtual void IncreaseWisdom(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["WisdomLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Wisdom : -Wisdom;

        ApplyOtherStat(dict, "WisdomLevel", Level);
    }

    //전투중 경험치 보석, 아이템을 획득하는 범위가 증가
    protected virtual void IncreaseTemptation(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["TemptationLevel"] >= dict["StatMaxLevel"]) return;

        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1f : (1f + (float)dict["DelicacyLevel"] / 10);

        //더해야될 레벨
        int Level = isPositive ? Temptation : Temptation;

        float change;

        if (dict["TemptationLevel"] + Level >= dict["StatMaxLevel"])
        {
            dict["TemptationLevel"] = dict["StatMaxLevel"];
            change = (dict["StatMaxLevel"] - dict["TemptationLevel"]) / 2;
        }
        else
        {
            dict["TemptationLevel"] += Level;
            change = Level / 2;
        }
        player.GetComponent<ItemTemptation>().range += change * DelicacyRate;
    }

    protected void IncreaseAll(Player player, bool isPositive = true)
    {
        IncreaseATK(player, isPositive);
        IncreaseDEF(player, isPositive);
        IncreaseSpeed(player, isPositive);
        IncreaseATKSpeed(player, isPositive);
        IncreaseATKRange(player, isPositive);
        //IncreaseATKDuration(player, isPositive);
        IncreaseCooldownReduction(player, isPositive);
        IncreaseGreed(player, isPositive);
        IncreaseDelicacy(player, isPositive);
        IncreaseWisdom(player, isPositive);
        IncreaseTemptation(player, isPositive);
    }

    public int ApplyAtkDefSpeed(Dictionary<string, int> dict, String statName, int level)
    {
        int change;
        if (dict[statName] + level >= dict["StatMaxLevel"])
        {
            dict[statName] = dict["StatMaxLevel"];
            change = dict["StatMaxLevel"] - dict[statName];
        }
        else
        {
            dict[statName] += level;
            change = level;
        }

        return change;
    }

    public void ApplyOtherStat(Dictionary<string, int> dict, string statName, int level)
    {
        if (dict[statName] + level >= dict["StatMaxLevel"])
        {
            dict[statName] = dict["StatMaxLevel"];
        }
        else dict[statName] += level;

        Debug.Log(dict[statName]+ " " + level + " " + dict["StatMaxLevel"]);
    }
}
