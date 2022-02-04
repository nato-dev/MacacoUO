using Server;
using System;
using System.Linq;


public class SavedSkill
{
    public SkillName skill;
    public double value;
    public int exp;
    public SkillLock Lock;

    public SavedSkill() { }
    public SavedSkill(SkillName skill, double value, int exp)
    {
        this.skill = skill;
        this.value = value;
        this.exp = exp;
    }

}

