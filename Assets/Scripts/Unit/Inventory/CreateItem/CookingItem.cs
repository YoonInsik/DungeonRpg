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
        if (player.HP + HP >= player.MaxHP || RecoveryFull == true)
        {
            player.HP = player.MaxHP;
        }
        else
        {
            player.HP += HP;
        }
    }

    //�ִ� ü�� ����
    public void IncreaseHP(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {
            player.MaxHP += MaxHP;
        }
        else
        {
            player.MaxHP -= MaxHP;
        }
    }

    //���ݷ� ����
    public void IncreaseATK(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {
            player.ATK += ATK;
        }
        else
        {
            player.ATK -= ATK;
        }
    }

    //���� ����
    public void IncreaseDEF(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {
            player.DEF += DEF;
        }
        else
        {
            player.DEF -= DEF;
        }
    }

    //������ ���ҷ� ����
    public void IncreaseFullnessDecrease(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {
            player.fullnessDecreaseAmount += 1;
        }
        else
        {
            player.fullnessDecreaseAmount -= 1;
        }   
    }

    //�̵��ӵ� ����
    public void IncreaseSpeed(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {
            player.speed += Speed;
        }
        else
        {
            player.speed -= Speed;
        }
    }

    //����ü �̵�,ȸ���ӵ�
    public void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //���ݹ���,����ü ũ�� ����
    public void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //��Ÿ�� ����
    public void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //������ �ߵ��Ҷ� �� ������ ���ӵǴ� �ð� ����
    public void IncreaseATKDuration(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //�������� Ŭ���� �� ��� ���� ����
    public void IncreaseGreed(Player player , bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //�丮����� ������ ������ �ɷ�ġ ����
    public void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //ȹ���ϴ� ����ġ ȹ�淮 ����
    public void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //������ ����ġ ����, �������� ȹ���ϴ� ������ ����
    public void IncreaseTemptation(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }
}
