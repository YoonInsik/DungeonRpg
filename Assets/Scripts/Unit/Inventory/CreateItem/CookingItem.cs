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

    //SHS ����
    [Multiline(4)]
    public string info_txt;

    //�丮 ������
    [Serializable]
    public struct Ingredient
    {
        public MeatItem ingredient;
        public int amount;
    }

    [SerializeField] public List<Ingredient> recipe;


    //�丮 ������ ȿ�� ����
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
            Debug.Log(name + "�� ������ �ƴ�");
        }
    }

    //����� ������ ȿ��
    protected abstract void AddEffect(Player player);
    //�������ӽð��� ���� �� ����� ȿ�� ����
    protected virtual void EndEffect(Player player) { }

    

    //ü�� ȸ��
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

    //�ִ� ü�� ����
    protected virtual void IncreaseHP(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;

        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1 : (1 + (float)dict["DelicacyLevel"] / 10);

        int change = isPositive ? MaxHP : -MaxHP;
        player.MaxHP += (int)(change * DelicacyRate);
    }

    //���ݷ� ����
    protected virtual void IncreaseATK(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKLevel"] >= dict["StatMaxLevel"]) return;

        int DelicacyRate = dict["DelicacyLevel"]*10;
       
        // ���� ����
        int Level = isPositive ? ATK : -ATK;
        int change = ApplyAtkDefSpeed(dict, "ATKLevel", Level);

        player.ATK += change * DelicacyRate;
    }

    //���� ����
    protected virtual void IncreaseDEF(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["DEFLevel"] >= dict["StatMaxLevel"]) return;

        int DelicacyRate = dict["DelicacyLevel"]*10;

        // ���� ����
        int Level = isPositive ? DEF : -DEF;
        int change = ApplyAtkDefSpeed(dict, "DEFLevel", Level);

        player.DEF += change * DelicacyRate;
    }

    //������ ���ҷ� ����
    protected virtual void IncreaseFullnessDecrease(Player player, bool isPositive = true)
    {
        int change = isPositive ? 1 : - 1;
        player.fullnessDecreaseAmount += change;
    }

    //�̵��ӵ� ����
    protected virtual void IncreaseSpeed(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["SpeedLevel"] >= dict["StatMaxLevel"]) return;

        //DelicacyLevel�� ���� ��ġ ����
        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1f : (1 + (float)dict["DelicacyLevel"] / 10);

        // ���� ����
        int Level = isPositive ? Speed : -Speed;
        int change = ApplyAtkDefSpeed(dict, "SpeedLevel", Level);
     
        player.speed += change * DelicacyRate;
    }

    //����ü �̵�,ȸ���ӵ�
    protected virtual void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKSpeedLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? ATKSpeed : ATKSpeed;

        ApplyOtherStat(dict, "ATKSpeedLevel", Level);
    }

    //���ݹ���,����ü ũ�� ����
    protected virtual void IncreaseATKRange(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["ATKRangeLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? ATKRange : -ATKRange;

        ApplyOtherStat(dict, "ATKRangeLevel", Level);
        
    }

    //��Ÿ�� ����
    protected virtual void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["CooldownReductionLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? CooldownReduction : -CooldownReduction ;

        ApplyOtherStat(dict, "CooldownReductionLevel", Level);
    }

    //������ �ߵ��Ҷ� �� ������ ���ӵǴ� �ð� ����
    //protected virtual void IncreaseATKDuration(Player player, bool isPositive = true)
    //{
    //    if (player.PlayerStatLevel.ATKdurationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

    //    int Level = isPositive ? 1 : -1;
    //    player.PlayerStatLevel.ATKdurationLevel += Level;
    //}

    //�������� Ŭ���� �� ��� ���� ����
    protected virtual void IncreaseGreed(Player player , bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["GreedLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Greed : -Greed;


        ApplyOtherStat(dict, "GreedLevel", Level);
    }

    //�丮����� ������ ������ �ɷ�ġ ����
    protected virtual void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["DelicacyLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Delicacy : -Delicacy;

        ApplyOtherStat(dict, "DelicacyLevel", Level);
    }
        
    //ȹ���ϴ� ����ġ ȹ�淮 ����
    protected virtual void IncreaseWisdom(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["WisdomLevel"] >= dict["StatMaxLevel"]) return;

        int Level = isPositive ? Wisdom : -Wisdom;

        ApplyOtherStat(dict, "WisdomLevel", Level);
    }

    //������ ����ġ ����, �������� ȹ���ϴ� ������ ����
    protected virtual void IncreaseTemptation(Player player, bool isPositive = true)
    {
        Dictionary<string, int> dict = player.StatLevels;
        if (dict["TemptationLevel"] >= dict["StatMaxLevel"]) return;

        float DelicacyRate = (dict["DelicacyLevel"] == 0) ? 1f : (1f + (float)dict["DelicacyLevel"] / 10);

        //���ؾߵ� ����
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
