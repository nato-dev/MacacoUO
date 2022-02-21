using System;
using System.Collections.Generic;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Commands
{
    public class Habs
    {
        public static void Initialize()
        {
            CommandSystem.Register("sethabilidades", AccessLevel.Administrator, new CommandEventHandler(CMD));
        }

        public class SetHabilidadesGump : Gump
        {
            public SetHabilidadesGump(BaseCreature bc)
                : base(12, 24)
            {
                this.Add(new GumpBackground(-12, -12, 1400, 800, 9200));

                var x = 0;
                var y = 0;
                var ct = 0;

                foreach (var a in WeaponAbility.Abilities)
                {
                    if (a != null)
                    {
                        AddHtml(10 + x, 10 + y, 200, 30, a.GetType().Name, false, false);

                    }
                    if (ct <= 3)
                    {
                        ct++;
                        x += 300;
                    }
                    else
                    {
                        ct = 0;
                        x = 0;
                        y += 30;
                    }
                }
                foreach (var a in WeaponAbility.Abilities)
                {
                    if (a != null)
                        AddHtml(10 + x, 10 + y, 200, 30, a.GetType().Name, false, false);
                    if (ct <= 3)
                    {
                        ct++;
                        x += 300;
                    }
                    else
                    {
                        ct = 0;
                        x = 0;
                        y += 30;
                    }
                }
                foreach (var a in WeaponAbility.Abilities)
                {
                    if (a != null)
                        AddHtml(10 + x, 10 + y, 200, 30, a.GetType().Name, false, false);
                    if (ct <= 3)
                    {
                        ct++;
                        x += 300;
                    }
                    else
                    {
                        ct = 0;
                        x = 0;
                        y += 30;
                    }
                }
            }
        }

        public static void CMD(CommandEventArgs arg)
        {
            var pl = arg.Mobile;
            pl.BeginTarget(-1, false, TargetFlags.None, new TargetCallback((Mobile m, object target) =>
            {
                if (!(target is BaseCreature))
                {
                    m.SendMessage("Precisa escolher um bixo");
                    return;
                }
                pl.SendGump(new SetHabilidadesGump(target as BaseCreature));
            }));
        }
    }
}
