using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Fronteira.Aspects
{
    public enum AspectType
    {
        Fogo, // +dano magico, +magic resist

        Gelo,  // +dano fisico, +acerto fisico

        Trovao, // +reflete dano, 

        Agua, // +dano pets, +cura pets

        Vento, // +esquiva, +velocidade atk

        Terra, // +parry, +armadura

        Luz, // +crafting +exp crafting

        Escuridao // +dano poison, +chance poison
    }
}
