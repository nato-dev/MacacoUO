using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Ziden.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitaNex.SuperGumps.UI;

namespace Server.Misc.Templates
{
    public class Template
    {
        public int Str;
        public int Dex;
        public int Int;
        public List<SavedSkill> savedSkills = new List<SavedSkill>();
        public string Name;
        public PlayerMobile owner;

        private double SetSkillValueAndExp(Skill s)
        {
            foreach (var savedSkill in savedSkills)
            {
                if (savedSkill.skill == s.SkillName)
                {
                    s.m_Exp = (ushort)savedSkill.exp;
                    s.Base = savedSkill.value;
                    s.SetLockNoRelay(savedSkill.Lock);
                }
            }
            return 0;
        }

        public void CopySkillsFromPlayer(PlayerMobile player)
        {
            // Resta as skills 1o
            foreach (var skill in player.Skills)
            {
                skill.SendPacket = false;
                skill.Base = 0;
                skill.SetLockNoRelay(SkillLock.Up);
                SetSkillValueAndExp(skill);
                skill.SendPacket = true;
            }
            player.RawInt = Int;
            player.RawDex = Dex;
            player.RawStr = Str;
            player.Send(new SkillUpdate(player.Skills));
            var remove = new List<BaseHire>();
            foreach (var f in player.AllFollowers)
            {
                if (f is BaseHire)
                {
                    var hire = (BaseHire)f;
                    remove.Add(hire);
                }
            }
            foreach(var hire in remove)
            {
                hire.Beg = false;
                hire.SetControlMaster(null);
                hire.Payday(hire);
                hire.Say("Voce se tornou uma pessoa diferente do que conheci... Nao posso mais lhe servir.");
            }
        }

        public bool ToPlayer(PlayerMobile player)
        {
            owner = player;
            bool confirm = false;
            foreach (var f in player.AllFollowers)
            {
                if(f is BaseHire)
                {
                    confirm = true;
                    break;
                }
            }
            Shard.Debug("Trocando template " + confirm);
            if(confirm)
            {
                player.SendGump(new ConfirmaTroca(player, this));
                return false;
              //  player.SendGump(new ConfirmDialogGump(player, null, 30, 30, "Atencao!", "Voce tem mercenarios contratados, eles irao perder o contrato se fizer isto", 7022, CopySkillsFromPlayer));
            } else
            {
                CopySkillsFromPlayer(player);
                return true;
            }

        }

        public void FromPlayer(PlayerMobile player)
        {
            savedSkills.Clear();
            foreach (var skill in player.Skills)
            {
                var saved = new SavedSkill();
                saved.skill = skill.SkillName;
                saved.exp = skill.m_Exp;
                saved.value = skill.Base;
                saved.Lock = skill.Lock;
                savedSkills.Add(saved);
            }
            Int = player.RawInt;
            Dex = player.RawDex;
            Str = player.RawStr;

        }
    }
}
