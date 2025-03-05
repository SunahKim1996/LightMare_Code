using UnityEngine;

public class PlayerData
{
    private int defensePower;
    public int DefensePower
    {
        get { return defensePower; }
        set
        {
            defensePower = value;
            //PlayerPrefs.SetInt("defensePower", defensePower);
        }
    }


    private int attackPower; 
    public int AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = value;
            //PlayerPrefs.SetFloat("power", power);
        }
    }


    private int specialAttackPower;
    public int SpecialAttackPower
    {
        get { return specialAttackPower; }
        set
        {
            specialAttackPower = value;
            //PlayerPrefs.SetFloat("specialPower", specialPower);
        }
    }


    public float SpecialAttackGaugeSpeed { get; set; } = 0.05f;

    private int spcialAttackGauge;
    public int SpecialAttackGauge
    {
        get { return spcialAttackGauge; }
        set 
        { 
            spcialAttackGauge = value;
            PlayerHUDManager.instance.AddSpecialAttackGauge(SpecialAttackGaugeSpeed, spcialAttackGauge);
        }
    }


    private int maxHp;
    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
            Hp = maxHp;

            //PlayerPrefs.SetFloat("maxHp", maxHp);
        }
    }

    private int hp;
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;

            if (hp > maxHp)
                hp = maxHp;

            PlayerHUDManager.instance.RefreshHPUI(hp, maxHp);
            //PlayerPrefs.SetFloat("Hp", hp);
        }
    } 

    private int exp = 0;
    public int Exp
    {
        get { return exp; }
        set
        {
            exp = value;

            PlayerHUDManager.instance.RefreshExpUI(exp);
        }
    }

    private int lightCount;
    public int LightCount
    {
        get { return lightCount; }
        set
        {
            lightCount = value;
            PlayerHUDManager.instance.RefreshLightCountUI(lightCount);

            //PlayerPrefs.SetInt("howMuchLight", lightCount);
        }
    }

    private int hpPoint;
    public int HpPoint
    {
        get { return hpPoint; }
        set
        {
            if (hpPoint >= 20)
                return;

            int preValue = hpPoint;
            hpPoint = value;


            int addValue = 5 * (hpPoint - preValue);
            MaxHp += addValue;
            Hp += addValue;

            //PlayerPrefs.SetInt("hpPoint", HpPoint);
        }
    }

    private int defensePoint;
    public int DefensePoint
    {
        get { return defensePoint; }
        set
        {
            if (defensePoint >= 20)
                return;

            int preValue = defensePoint;
            defensePoint = value;

            DefensePower += 2 * (defensePoint - preValue);

            //PlayerPrefs.SetInt("defensePoint", DefensePoint);
        }
    }

    private int chargeSpeedPoint;
    public int ChargeSpeedPoint
    {
        get { return chargeSpeedPoint; }
        set
        {
            if (chargeSpeedPoint >= 20)
                return;

            int preValue = chargeSpeedPoint;
            chargeSpeedPoint = value;

            SpecialAttackGaugeSpeed += 0.005F * (chargeSpeedPoint - preValue);

            //PlayerPrefs.SetInt("chargeSpeedPoint", ChargeSpeedPoint);
        }
    }

    private int attackPoint;
    public int AttackPoint
    {
        get { return attackPoint; }
        set
        {
            if (attackPoint >= 20)
                return;

            int preValue = attackPoint;
            attackPoint = value;

            int addValue = (attackPoint - preValue);
            AttackPower += 2 * addValue;
            specialAttackPower += 10 * addValue;

            //PlayerPrefs.SetInt("AttackPoint", AttackPoint);
        }
    }

    private int recentMap = 1;
    public int RecentMap
    {
        get { return recentMap; }
        set 
        {  
            recentMap = value;
            PlayerPrefs.SetInt("RecentMapNumber", value);
        }
    }
}
