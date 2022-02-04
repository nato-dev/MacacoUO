using System;
using Server.Mobiles;
using Server.Multis;
using System.Collections.Generic;
using System.Linq;
using Server.ContextMenus;
using Server.Regions;

namespace Server.Commands
{
    public class Musicaqui
    {
        public static void Initialize()
        {
            CommandSystem.Register("musicaqui", AccessLevel.Administrator, DebugarCmd);
        }

        public static MusicName GetMusicaParents(Region r)
        {
            if (r == null)
                return MusicName.Invalid;

            Shard.Debug("-- Vendo " + r.Name);
            while (r.Parent != null && r.Parent.Name != null)
            {
                Shard.Debug("-- Parent " + r.Parent.Name + " Music " + r.Music);
                if (r != null && r.Name != null && r.Music != MusicName.Invalid)
                    return r.Music;
                r = r.Parent;
            }
            return Region.DEFAULT;
        }

        public static MusicName GetMusicaCerteira(Region r)
        {
            if (r == null)
                return MusicName.Invalid;


            var imWild = r == null || r.Name == null || r.Music == Region.DEFAULT || r.Music == MusicName.Invalid;

            if (Shard.DebugEnabled)
                Shard.Debug("- Regiao nao tem musica? " + imWild);


            if (imWild)
            {

                if (Shard.DebugEnabled)
                    Shard.Debug("- Tentando pegar musica do parent " + r.Name);

                var musicaParent = GetMusicaParents(r);

                if (Shard.DebugEnabled)
                    Shard.Debug("- Musica Parent " + musicaParent);

                if (musicaParent != MusicName.Invalid)
                {
                    Shard.Debug("- Achei  musica parent valida " + musicaParent);
                    return musicaParent;
                }
                Shard.Debug("- Esse stack de regiao eh wild mas nao tem musica");
                return MusicName.Invalid;
            }
            Shard.Debug("- Nao eh wild");
            return MusicName.Invalid;
        }

        public static MusicName GetMusic(Mobile m, Region r)
        {
            if (Shard.DebugEnabled)
                Shard.Debug("Pegando musica da regiao " + r.Name);

            var musica = GetMusicaCerteira(r);

            if (musica == MusicName.Invalid)
                if (m.Location.X < 5100)
                    return Region.FLORESTA;
                else
                    return Region.CAVERNA;
            else
                return musica;
        }

        public static void DebugarCmd(CommandEventArgs t)
        {
            var musica = GetMusic(t.Mobile, t.Mobile.Region);
            t.Mobile.SendMessage("Musica aqui: " + musica);
        }
    }
}
