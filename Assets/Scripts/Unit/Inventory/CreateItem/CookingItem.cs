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
    public IEnumerator CookingEffect(Player player)
    {
        AddEffect(player);
        if (buffDuration != 0)
        {
            yield return new WaitForSeconds(buffDuration);
            EndEffect(player);
            Debug.Log(name + " " + buffDuration + "�� Buff��");
        }
        else
        {
            Debug.Log(name + "�� ������ �ƴ�");
        }
    }

    //����� ������ ȿ��
    public abstract void AddEffect(Player player);
    //�������ӽð��� ���� �� ����� ȿ�� ����
    public virtual void EndEffect(Player player) { }


    //ü�� ȸ��
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

    //�ִ� ü�� ����
    public void IncreaseHP(Player player, bool isPositive = true)
    {
        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1 : (1 + (float)player.PlayerStatLevel.DelicacyLevel / 10);
    
        int change = isPositive ? MaxHP : -MaxHP;
        player.MaxHP += (int)(change * DelicacyRate);
    }

    //���ݷ� ����
    public void IncreaseATK(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel;

        int change = isPositive ? ATK : -ATK;
        player.ATK += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKLevel += Level;
    }

    //���� ����
    public void IncreaseDEF(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DEFLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int DelicacyRate = player.PlayerStatLevel.DelicacyLevel;

        int change = isPositive ? DEF : -DEF;
        player.DEF += change + DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.DEFLevel += Level;
    }

    //������ ���ҷ� ����
    public void IncreaseFullnessDecrease(Player player, bool isPositive = true)
    {
        int change = isPositive ? 1 : - 1;
        player.fullnessDecreaseAmount += change;
    }

    //�̵��ӵ� ����
    public void IncreaseSpeed(Player player, bool isPositive = true)
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
    public void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKSpeedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKSpeedLevel += Level;
    }

    //���ݹ���,����ü ũ�� ����
    public void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKRangeLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKRangeLevel += Level;
    }

    //��Ÿ�� ����
    public void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.CooldownReductionLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.CooldownReductionLevel += Level;
    }

    //������ �ߵ��Ҷ� �� ������ ���ӵǴ� �ð� ����
    public void IncreaseATKDuration(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.ATKdurationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.ATKdurationLevel += Level;
    }

    //�������� Ŭ���� �� ��� ���� ����
    public void IncreaseGreed(Player player , bool isPositive = true)
    {
        if (player.PlayerStatLevel.GreedLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        if (isPositive == true)
        {

        }
        else
        {

        }

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.GreedLevel += Level;
    }

    //�丮����� ������ ������ �ɷ�ġ ����
    public void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.DelicacyLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.DelicacyLevel += Level;
    }

    //ȹ���ϴ� ����ġ ȹ�淮 ����
    public void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.WisdomLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.WisdomLevel += Level;
    }

    //������ ����ġ ����, �������� ȹ���ϴ� ������ ����
    public void IncreaseTemptation(Player player, bool isPositive = true)
    {
        if (player.PlayerStatLevel.TemptationLevel >= player.PlayerStatLevel.StatMaxLevel) return;

        float DelicacyRate = (player.PlayerStatLevel.DelicacyLevel == 0) ? 1f : (1f + (float)player.PlayerStatLevel.DelicacyLevel / 10);

        float change = isPositive ? Temptation / 2 : -Temptation / 2;
        player.GetComponent<ItemTemptation>().range += change * DelicacyRate;

        int Level = isPositive ? 1 : -1;
        player.PlayerStatLevel.TemptationLevel += Level;
    }
}
