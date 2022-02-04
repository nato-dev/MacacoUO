using System;
using Server.Targeting;

namespace Server.Engines.PartySystem
{
    public class AddPartyTarget : Target
    {
        public AddPartyTarget(Mobile from)
            : base(8, false, TargetFlags.None)
        {
            from.SendMessage("Quem voce quer adicionar ao grupo?"); // Who would you like to add to your party?
        }

        protected override void OnTarget(Mobile from, object o)
        {
            if (o is Mobile)
            {
                Mobile m = (Mobile)o;
                Party p = Party.Get(from);
                Party mp = Party.Get(m);

                if (from == m)
                    from.SendMessage("Voce nao pode adicionar a si mesmo no grupo"); // You cannot add yourself to a party.
                else if (p != null && p.Leader != from)
                    from.SendMessage("Voce so pode adicionar membros se for o lider"); // You may only add members to the party if you are the leader.
                else if (m.Party is Mobile)
                    return;
                else if (p != null && (p.Members.Count + p.Candidates.Count) >= Party.Capacity)
                    from.SendMessage("So pode ter 10 num grupo"); // You may only have 10 in your party (this includes candidates).
                else if (!m.Player && m.Body.IsHuman)
                    m.SayTo(from, "Nah obrigado"); // Nay, I would rather stay here and watch a nail rust.
                else if (!m.Player)
                    from.SendMessage("A criatura te ignora... oxente..."); // The creature ignores your offer.
                else if (mp != null && mp == p)
                    from.SendMessage("Esta pessoa ja esta em seu grupo"); // This person is already in your party!
                else if (mp != null)
                    from.SendMessage("Pessoa ja esta em um grupo"); // This person is already in a party!
                else
                    Party.Invite(from, m);
            }
            else
            {
                from.SendMessage("Vai sair com isso em um grupo para uma aventura ?"); // You may only add living things to your party!
            }
        }
    }
}
