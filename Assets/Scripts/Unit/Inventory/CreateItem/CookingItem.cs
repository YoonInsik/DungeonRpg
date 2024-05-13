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
        if (player.HP + HP >= player.MaxHP || RecoveryFull == true)
        {
            player.HP = player.MaxHP;
        }
        else
        {
            player.HP += HP;
        }
    }

    //최대 체력 증가
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

    //공격력 증가
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

    //방어력 증가
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

    //포만감 감소량 증가
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

    //이동속도 증가
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

    //투사체 이동,회전속도
    public void IncreaseATKSpeed(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //공격범위,투사체 크기 증가
    public void IncreaseATKRange(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //쿨타임 감소
    public void IncreaseCooldownReduction(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //공격이 발동할때 그 공격이 지속되는 시간 증가
    public void IncreaseATKDuration(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //스테이지 클리어 후 얻는 재료양 증가
    public void IncreaseGreed(Player player , bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //요리섭취시 레벨당 오르는 능력치 증가
    public void IncreaseDelicacy(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //획득하는 경험치 획득량 증가
    public void IncreaseWisdom(Player player, bool isPositive = true)
    {
        if (isPositive == true)
        {

        }
        else
        {

        }
    }

    //전투중 경험치 보석, 아이템을 획득하는 범위가 증가
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
