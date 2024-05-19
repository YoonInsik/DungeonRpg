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

    //�ִ� ü�� ����
    protected virtual void IncreaseHP(Player player, bool isPositive = true)
    {
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1 : (1 + (float)player.PlayerStatLevel.DelicacyLevel / 10);
    
        int change = isPositive ? MaxHP : -MaxHP;
        player.MaxHP += (int)(change * DelicacyRate);
    }

    //���ݷ� ����
    protected virtual void IncreaseATK(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel*10;
        int change;

        // ���� ����
        int Level = isPositive ? ATK : -ATK;

        if (player.PlayerStatLevel.ATKLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.ATKLevel = player.PlayerStatLevel.StatMaxLevel;
            change = player.PlayerStatLevel.StatMaxLevel - player.PlayerStatLevel.ATKLevel;
        }
        else
        {
            player.PlayerStatLevel.ATKLevel += Level;
            change = Level;
        }
        player.ATK += change * DelicacyRate;
    }

    //���� ����
    protected virtual void IncreaseDEF(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DEFLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel*10;
        int change;

        // ���� ����
        int Level = isPositive ? DEF : -DEF;

        if (player.PlayerStatLevel.DEFLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.DEFLevel = player.PlayerStatLevel.StatMaxLevel;
            change = player.PlayerStatLevel.StatMaxLevel - player.PlayerStatLevel.DEFLevel;
        }
        else
        {
            player.PlayerStatLevel.DEFLevel += Level;
            change = Level;
        }
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
        if (player.PlayerStatLevel.SpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        //�̽Ŀ� ���� ��ġ ����
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);
        int change;

        // ���� ����
        int Level = isPositive ? Speed : -Speed;

        if (player.PlayerStatLevel.SpeedLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.SpeedLevel = player.PlayerStatLevel.StatMaxLevel;
            change = player.PlayerStatLevel.StatMaxLevel - player.PlayerStatLevel.SpeedLevel;
        }
        else
        {
            player.PlayerStatLevel.SpeedLevel += Level;
            change = Level;
        }
        player.speed += change * DelicacyRate;
    }

    //����ü �̵�,ȸ���ӵ�
    protected virtual void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKSpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;

        if (player.PlayerStatLevel.ATKSpeedLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.ATKSpeedLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.ATKSpeedLevel += Level;
    }

    //���ݹ���,����ü ũ�� ����
    protected virtual void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKRangeLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? ATKRange : -ATKRange;

        if (player.PlayerStatLevel.ATKRangeLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.ATKRangeLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.ATKRangeLevel += Level;
    }

    //��Ÿ�� ����
    protected virtual void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.CooldownReductionLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? CooldownReduction : -CooldownReduction ;

        if (player.PlayerStatLevel.CooldownReductionLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.CooldownReductionLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.CooldownReductionLevel += Level;
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
        if (player.PlayerStatLevel.GreedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? Greed : -Greed;


        if (player.PlayerStatLevel.GreedLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.GreedLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.GreedLevel += Level;
    }

    //�丮����� ������ ������ �ɷ�ġ ����
    protected virtual void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DelicacyLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? Delicacy : -Delicacy;

        if (player.PlayerStatLevel.DelicacyLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.DelicacyLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.DelicacyLevel += Level;
    }

    //ȹ���ϴ� ����ġ ȹ�淮 ����
    protected virtual void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.WisdomLevel >= player.PlayerStatLevel.StatMaxLevel) return;  

        int Level = isPositive ? Wisdom : -Wisdom;

        if (player.PlayerStatLevel.WisdomLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.WisdomLevel = player.PlayerStatLevel.StatMaxLevel;
        }
        else player.PlayerStatLevel.WisdomLevel += Level;
    }

    //������ ����ġ ����, �������� ȹ���ϴ� ������ ����
    protected virtual void IncreaseTemptation(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.TemptationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        //���ؾߵ� ����
        int Level = isPositive ? Temptation : Temptation;

        float change;

        if (player.PlayerStatLevel.TemptationLevel + Level >= player.PlayerStatLevel.StatMaxLevel)
        {
            player.PlayerStatLevel.TemptationLevel = player.PlayerStatLevel.StatMaxLevel;
            change = (player.PlayerStatLevel.StatMaxLevel - player.PlayerStatLevel.TemptationLevel) / 2;
        }
        else
        {
            player.PlayerStatLevel.TemptationLevel += Level;
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
}
