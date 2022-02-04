
using Server;
using Server.Engines.PartySystem;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Felladrin.Automations
{
    public static class DivideGold
    {

        public static List<PartyMemberInfo> DivideQuanto(Mobile from)
        {
            var party = Party.Get(from);
            if (party == null)
                return null;

            var mesmaRegiao = new List<PartyMemberInfo>();
            foreach (var info in party.Members)
            {
                if (!(info.Mobile is PlayerMobile))
                    continue;

                var partyMember = info.Mobile as PlayerMobile;
                if (partyMember == null)
                    continue;

                if (partyMember.Region == from.Region && partyMember.GetDistance(from) < 30)
                    mesmaRegiao.Add(info);
            }
            return mesmaRegiao;
        }

        public static bool Divide(Mobile from, Item item)
        {
            if (item == null)
                return false;

            if (from == null)
                return false;

            if (!(item is Gold) || from.Party == null || from.Party is Party && item.Amount < ((Party)from.Party).Members.Count)
                return false;

            var party = Party.Get(from);
            if (party == null)
                return false;

            var mesmaRegiao = DivideQuanto(from);

            int share = item.Amount / mesmaRegiao.Count;

            //share = (int)(share * 1.2);
            //var bonus = (int)(share * 0.2);

            foreach (var info in mesmaRegiao)
            {

                if(!info.Mobile.IsCooldown("dicamoney"))
                {
                    info.Mobile.SetCooldown("dicamoney", TimeSpan.FromHours(2));
                    info.Mobile.SendMessage(78, "Voce ganha 20% a mais de gold por estar em um grupo !");
                }

                var partyMember = info.Mobile as PlayerMobile;

                if (partyMember == null || partyMember.Backpack == null)
                    continue;

                var receiverGold = partyMember.Backpack.FindItemByType<Gold>();

                if (partyMember != from)
                {
                    if (receiverGold != null)
                        receiverGold.Amount += share;
                    else
                        partyMember.Backpack.DropItem(new Gold(share));
                }
                   
                //partyMember.PlaySound(item.GetDropSound());

                if (partyMember == from)
                {
                    from.SendMessage("Voce pegou o dinheiro e distribuiu com o grupo, {0}gp para cada.", share);

                    int rest = item.Amount % mesmaRegiao.Count;

                    if (rest > 0)
                    {
                        var sharerGold = from.Backpack.FindItemByType<Gold>();

                        if (sharerGold != null)
                            sharerGold.Amount += rest;
                        else
                            from.Backpack.DropItem(new Gold(rest));

                        from.SendMessage("Voce ficou com {0} moedas que sobraram.", rest);
                    }
                }
                else
                {
                    partyMember.SendMessage("{0} looteou e distribuiu {1}gp para cada no grupo.", from.Name, share);

                    if (WeightOverloading.IsOverloaded(partyMember))
                    {
                        if(receiverGold != null)
                            receiverGold.Amount -= share;

                        var sharerGold = from.Backpack.FindItemByType<Gold>();

                        if (sharerGold != null)
                            sharerGold.Amount += share;
                        else
                            from.Backpack.DropItem(new Gold(share));

                        partyMember.SendMessage("Mas {0} ficou com sua parte pois voce esta muito pesado.", (from.Female ? "she" : "he"));

                        from.SendMessage("Voce ficou com a parte de {0}'s por que ele esta muito pesado.", partyMember.Name);
                    }
                }
            }

            item.Amount = share;
            //item.Delete();

            return false;
        }
    }
}
