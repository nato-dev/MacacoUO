using Server.Commands;
using Server.Mobiles;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden.Traducao
{
    class Trads
    {
        public static void Initialize()
        {
            CommandSystem.Register("traduzir", AccessLevel.VIP, OnAction);
        }

        public static void Configure()
        {
            Console.WriteLine("Inicializando listener traducoes");
            EventSink.ItemCreated += ItemCriado;
            EventSink.MobileCreated += MobileCriado;
        }

        public static void MobileCriado(MobileCreatedEventArgs e)
        {
            if (Mobiles.ContainsKey(e.Mobile.GetType().Name))
            {
                e.Mobile.Name = Mobiles[e.Mobile.GetType().Name].ToLower();
            }
        }

        public static void UpdateNomeItem(Item i)
        {
            string nome = null;

            // Sistema novo galera traduz ingame
            if (Items.TryGetValue(i.GetType().Name, out nome))
            {
                i.Name = nome;
            }

            // Sistema velho, traduz na unha
            var itemName = Lang.ItemTrans(i.GetType().Name);
            if (itemName != null)
            {
                i.Name = itemName.ToLower();
            }

            if (i.Name == null && i.DefaultName != null)
            {
                i.Name = i.DefaultName;
            }
        }

        public static void ItemCriado(ItemCreatedEventArgs e)
        {
            if (Shard.DebugGritando)
            {
                Shard.Debug("Item Criado " + e.Item.GetType().Name);
            }

            UpdateNomeItem(e.Item);
            /*
            var nome = GetNome(e.Item.GetType());
            if (nome != null)
            {
                e.Item.Name = nome;
            }
            */
        }

        public static Dictionary<Type, string> ItemTrans = new Dictionary<Type, string>();
        public static HashSet<Type> Ignore = new HashSet<Type>();

        public static String GetNome(Type type)
        {
            if (Ignore.Contains(type))
                return null;

            string nome = null;
            if (ItemTrans.TryGetValue(type, out nome))
            {
                return nome;
            }

            try
            {
                Item.BypassInitialization = true;
                var i = (Item)Activator.CreateInstance(type);
                Item.BypassInitialization = false;
                if (i != null)
                {
                    UpdateNomeItem(i);
                    if (i.Name != null)
                    {
                        ItemTrans.Add(type, i.Name);
                        return i.Name;
                    }
                    else
                    {
                        Ignore.Add(type);
                    }

                }
            }
            catch
            {
                Shard.Erro("Erro ao traduzir item " + type.Name);
                Item.BypassInitialization = false;
            }
            finally
            {
                Item.BypassInitialization = false;
            }
            return null;
        }


        public static Dictionary<Serial, int> Tradutores = new Dictionary<Serial, int>();
        public static Dictionary<string, string> Items = new Dictionary<string, string>();
        public static Dictionary<string, string> Mobiles = new Dictionary<string, string>();
        public static Dictionary<int, string> Clilocs = new Dictionary<int, string>();

        public static void Score(Mobile m)
        {
            if (!Tradutores.ContainsKey(m.Serial))
            {
                Tradutores[m.Serial] = 1;
            }
            else
            {
                Tradutores[m.Serial]++;
            }
        }

        [Usage("traduzir")]
        private static void OnAction(CommandEventArgs e)
        {
            if (e.Arguments.Count() < 1)
            {
                e.Mobile.SendMessage("Use /traduzir 'nome da coisa' ou /traduzir <numero cliloc> 'texto' para traduzir");
                return;
            }
            else if (e.Arguments.Count() == 1)
            {
                var texto = e.Arguments[0];
                e.Mobile.SendMessage("Selecione o monstro/item que deseja traduzir");
                e.Mobile.Target = new IT(texto);
            }
            else if (e.Arguments.Count() == 2)
            {
                try
                {
                    var cliloc = Int32.Parse(e.Arguments[0]);
                    var texto = e.Arguments[1];

                    var trans = Lang.Trans(cliloc);
                    var temloc = Clilocs.ContainsKey(cliloc);
                    var trad = trans;
                    if (trad == null && temloc)
                        trad = Clilocs[cliloc];
                    if (trad != null && e.Mobile.LastTraducao != "CL_" + cliloc)
                    {
                        e.Mobile.SendMessage("Este cliloc ja esta traduzido para: " + trad);
                        e.Mobile.SendMessage("Use o comando novamente para sobre-escrever");
                        e.Mobile.LastTraducao = "CL_" + cliloc;
                    }
                    else
                    {
                        Clilocs[cliloc] = texto;
                        e.Mobile.SendMessage("Cliloc " + cliloc + " traduzido para: " + texto);
                        Score(e.Mobile);
                    }
                }
                catch (Exception ex)
                {
                    e.Mobile.SendMessage("Formatacao invalida, use /traduzir <numero> ''texto''");
                }

            }


        }

        public class IT : Target
        {

            private string texto;

            public IT(string texto) : base(10, true, TargetFlags.None)
            {
                this.texto = texto;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item)
                {
                    var item = (Item)targeted;
                    var tipo = item.GetType().Name;
                    if (Items.ContainsKey(tipo))
                    {
                        if (from.LastTraducao == tipo)
                        {
                            from.SendMessage("Item traduzido para " + texto);
                            Items[tipo] = texto;
                            item.Name = texto;
                            Score(from);
                        }
                        else
                        {
                            var trad = Items[tipo];
                            from.SendMessage("Este item ja tem uma traducao: " + trad);
                            from.SendMessage("Use o comando novamente no item para sobreescrever");
                            from.LastTraducao = tipo;
                        }

                    }
                    else
                    {
                        from.SendMessage("Item traduzido para " + texto);
                        if (texto == "")
                        {
                            Items[tipo] = texto;
                            item.Name = texto;
                        }
                        else
                        {
                            Items.Remove(tipo);
                        }

                        Score(from);
                    }
                }
                else if (targeted is BaseCreature)
                {
                    var bc = (BaseCreature)targeted;
                    var tipo = bc.GetType().Name;
                    if (Mobiles.ContainsKey(tipo))
                    {
                        if (texto == "")
                        {
                            Mobiles.Remove(tipo);
                            bc.Name = null;
                            from.SendMessage("Mobile removido");
                            return;
                        }
                        if (from.LastTraducao == tipo)
                        {
                            from.SendMessage("Monstro traduzido para " + texto);
                            Mobiles[tipo] = texto;
                            bc.Name = texto;
                            Score(from);
                        }
                        else
                        {
                            var trad = Mobiles[tipo];
                            from.SendMessage("Este monstro ja tem uma traducao: " + trad);
                            from.SendMessage("Use o comando novamente no monstro para sobreescrever");
                            from.LastTraducao = tipo;
                        }
                    }
                    else
                    {
                        from.SendMessage("Monstro traduzido para " + texto);
                        if (texto == "")
                        {
                            Mobiles.Remove(tipo);
                        }
                        else
                        {
                            Mobiles[tipo] = texto;
                            bc.Name = texto;
                            Score(from);
                        }
                    }
                }
                else
                {
                    from.SendMessage("Por enquanto so pode-se traduzir items/monstros ou clilocs");
                }
            }
        }
    }
}
