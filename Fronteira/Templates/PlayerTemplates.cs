using Server;
using Server.Misc.Templates;
using Server.Mobiles;
using System;
using System.Collections.Generic;

public class PlayerTemplates
{
    private static int NULL_BIT = 0xFF;

    public List<Template> Templates = new List<Template>();

    public bool NomeJaExiste(PlayerMobile player, string name)
    {
        var current = GetCurrentTemplate(player);
        foreach (var template in Templates)
        {
            if (template != current)
            {
                if (name == template.Name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Template GetCurrentTemplate(PlayerMobile player)
    {
        var name = player.CurrentTemplate;
        foreach(var template in Templates)
        {
            if (template.Name == name)
                return template;
        }
        return player.SetDefaultTemplate();
    }

    public void Serialize(GenericWriter writer)
    {
        writer.Write(2); // version
        writer.Write(Templates.Count);
        foreach(var template in Templates)
        {
            writer.Write(template.Name);
            writer.Write(template.Str);
            writer.Write(template.Dex);
            writer.Write(template.Int);
            writer.Write(template.savedSkills.Count);
            foreach (var skill in template.savedSkills)
            {
                writer.Write((int)skill.skill);
                writer.Write(skill.value);
                writer.Write(skill.exp);
                writer.Write((ushort)skill.Lock);
            }
        }
    }

    public void Deserialize(GenericReader reader)
    {
        var version = reader.ReadInt();

        var numberOfTemplates = reader.ReadInt();
        for (var i = 0; i < numberOfTemplates; i++)
        {
            var template = new Template();
            template.Name = reader.ReadString();
            template.Str = reader.ReadInt();
            template.Dex = reader.ReadInt();
            template.Int = reader.ReadInt();
            var count = reader.ReadInt();
            for (var s = 0; s < count; s++)
            {
                var skill = new SavedSkill();
                skill.skill = (SkillName)reader.ReadInt();
                skill.value = reader.ReadDouble();
                skill.exp = reader.ReadInt();
                if(version >= 2)
                {
                    skill.Lock = (SkillLock)reader.ReadUShort();
                }
                template.savedSkills.Add(skill);

            }
            Templates.Add(template);
        }

    }
}

