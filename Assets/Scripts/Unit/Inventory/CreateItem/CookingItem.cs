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
        int change = isPositive ? ATK : -ATK;
        player.ATK += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKLevel += Level;
    }

    //���� ����
    protected virtual void IncreaseDEF(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DEFLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel*10;
        int change = isPositive ? DEF : -DEF;
        player.DEF += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.DEFLevel += Level;
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
        float change = isPositive ? Speed : -Speed;
        player.speed += change*DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.SpeedLevel += Level;
    }

    //����ü �̵�,ȸ���ӵ�
    protected virtual void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKSpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKSpeedLevel += Level;
    }

    //���ݹ���,����ü ũ�� ����
    protected virtual void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKRangeLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKRangeLevel += Level;
    }

    //��Ÿ�� ����
    protected virtual void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.CooldownReductionLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.CooldownReductionLevel += Level;
    }

    //������ �ߵ��Ҷ� �� ������ ���ӵǴ� �ð� ����
    protected virtual void IncreaseATKDuration(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKdurationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKdurationLevel += Level;
    }

    //�������� Ŭ���� �� ��� ���� ����
    protected virtual void IncreaseGreed(Player player , bool isPositive = true)
    {
        if (player.PlayerStatLevel.GreedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.GreedLevel += Level;
    }

    //�丮����� ������ ������ �ɷ�ġ ����
    protected virtual void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DelicacyLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.DelicacyLevel += Level;
    }

    //ȹ���ϴ� ����ġ ȹ�淮 ����
    protected virtual void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.WisdomLevel >= player.PlayerStatLevel.StatMaxLevel) return;  

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.WisdomLevel += Level;
    }

    //������ ����ġ ����, �������� ȹ���ϴ� ������ ����
    protected virtual void IncreaseTemptation(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.TemptationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);
        float change = isPositive ? Temptation / 2 : -Temptation / 2;
        player.GetComponent<ItemTemptation>().range += change * DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.TemptationLevel += Level;
    }

    protected void IncreaseAll(Player player, bool isPositive = true)
    {
        IncreaseATK(player, isPositive);
        IncreaseDEF(player, isPositive);
        IncreaseSpeed(player, isPositive);
        IncreaseATKSpeed(player, isPositive);
        IncreaseATKRange(player, isPositive);
        IncreaseATKDuration(player, isPositive);
        IncreaseCooldownReduction(player, isPositive);
        IncreaseGreed(player, isPositive);
        IncreaseDelicacy(player, isPositive);
        IncreaseWisdom(player, isPositive);
        IncreaseTemptation(player, isPositive);
    }
}
