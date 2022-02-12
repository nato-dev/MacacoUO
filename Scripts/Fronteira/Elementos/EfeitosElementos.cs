using Server.Items;
using System.Collections.Generic;

namespace Server.Fronteira.Elementos
{
    public class EfeitosElementos
    {
        private static Dictionary<ElementoPvM, string[]> _efeitos = new Dictionary<ElementoPvM, string[]>();
        private static Dictionary<ElementoPvM, string[]> _efeitosColar = new Dictionary<ElementoPvM, string[]>();

        public static void Effect(Mobile m, ElementoPvM e)
        {
            m.FixedParticles(0x3779, 8, 10, 5002, BaseArmor.HueElemento(e), 0, EffectLayer.Head, 1);
        }

        public static string [] GetEfeitos(ElementoPvM elemento)
        {
            string[] efeitos;
            if (_efeitos.TryGetValue(elemento, out efeitos))
                return efeitos;

            switch(elemento)
            {
                case ElementoPvM.Fogo:
                    efeitos = new string[] {
                        "Dano de Fogo",
                        "Esquiva",
                        "Fogo Queima"
                    };
                    break;
                case ElementoPvM.Agua:
                    efeitos = new string[] {
                        "Dano Pocoes",
                        "Magic Resist",
                        "Dano Eletrico",
                    };
                    break;
                case ElementoPvM.Terra:
                    efeitos = new string[] {
                        "Dano & Resist de Venenos",
                        "Armadura",
                        "Dano Fisico"
                    };
                    break;
                case ElementoPvM.Raio:
                    efeitos = new string[] {
                        "Dano Eletrico",
                        "Dano Fisico",
                        "Esquiva"
                    };
                    break;
                case ElementoPvM.Luz:
                    efeitos = new string[] {
                        "Cura ao Atacar",
                        "Resistencia Magica",
                        "Armadura"
                    };
                    break;
                case ElementoPvM.Escuridao:
                    efeitos = new string[] {
                        "Penetr. Magica",
                        //"Dano Magias Proibidas",
                        "Resistencia Magica",
                        "LifeSteal Magico"
                    };
                    break;
                case ElementoPvM.Gelo:
                    efeitos = new string[] {
                        "Esquiva",
                        "Resistencia Magica",
                        "Bonus Coleta Recursos"
                    };
                    break;
                case ElementoPvM.Vento:
                    efeitos = new string[] {
                        "Velocidade Ataque",
                        "Penetr. Armadura",
                        "Esquiva"
                    };
                    break;
                default:
                    efeitos = new string[] { };
                    break;
            }
            _efeitos[elemento] = efeitos;
            return efeitos;
        }

        public static string[] GetEfeitosColar(ElementoPvM elemento)
        {
            string[] efeitos;
            if (_efeitosColar.TryGetValue(elemento, out efeitos))
                return efeitos;

            switch (elemento)
            {
                case ElementoPvM.Fogo:
                    efeitos = new string[] {
                        "Bonus Flamestrike", //
                        "Bonus Fire Field",  //
                    };
                    break;
                case ElementoPvM.Agua:
                    efeitos = new string[] {
                        "Bonus Pots de Dano", //
                        "Magic Resist",       //
                    };
                    break;
                case ElementoPvM.Terra:
                    efeitos = new string[] {
                        "Armor Pets",       //
                        "Dano Fisico Pets", //
                    };
                    break;
                case ElementoPvM.Raio:
                    efeitos = new string[] {
                        "Bonus Energy Bolt", //
                        "Bonus Lightning",   //
                    };
                    break;
                case ElementoPvM.Luz:
                    efeitos = new string[] {
                        "Chance Resist a Morte", //
                        "Parry Bloqueia Magias", //
                    };
                    break;
                case ElementoPvM.Escuridao:
                    efeitos = new string[] {
                        "Bonus Resist Magias Negras", //
                        "Bonus Magias Negras",        //
                    };
                    break;
                case ElementoPvM.Gelo:
                    efeitos = new string[] {
                        "Bonus Magias de Varinhas",   //
                        "Chance Congelar Monstros Atacantes"      // 
                    };
                    break;
                case ElementoPvM.Vento:
                    efeitos = new string[] {
                        "Chance Critico",       //
                        "Chance Stun"           //
                    };
                    break;
                default:
                    efeitos = new string[] { };
                    break;
            }
            _efeitosColar[elemento] = efeitos;
            return efeitos;
        }
    }
}
