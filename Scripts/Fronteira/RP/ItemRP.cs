
namespace Server.Fronteira.RP
{
    /*
    public class ItemRP
    {
        public static void Configure()
        {
            EventSink.CheckEquipItem += CheckEquip;
            EventSink.CraftSuccess += Craft;
            EventSink.CreatureDeath += BixoMorre;
        }

        public static void BixoMorre(CreatureDeathEventArgs e)
        {
            var bc = e.Creature as BaseCreature;
            if(bc != null)
            {
                var rights = bc.GetLootingRights();
                var rps = rights.Where(r => r.m_Mobile != null && r.m_Mobile.RP).Count();
                // Mais da metade do grupo eh RP
                if(rps > Math.Floor(rights.Count / 2d))
                {
                    foreach (var i in e.Corpse.Items)
                        i.RP = true;
                }
            }
        }

        public static void Craft(CraftSuccessEventArgs e)
        {
            if (e.Crafter.RP)
                e.CraftedItem.RP = true;
        }

        public static void CheckEquip(CheckEquipItemEventArgs e)
        {

        }
     
    }
    */
}
