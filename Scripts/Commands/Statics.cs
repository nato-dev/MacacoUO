using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Server.Commands;
using Server.Gumps;
using Server.Items;

namespace Server
{
    public class Statics
    {
        private static readonly Point3D NullP3D = new Point3D(int.MinValue, int.MinValue, int.MinValue);
        private static byte[] m_Buffer;
        private static StaticTile[] m_TileBuffer = new StaticTile[128];
        private const string BaseFreezeWarning = "{0}  " +
                                                 "Esses itens <u> serão removidos do mundo </u> e colocados nos arquivos de dados do servidor.  " +
                                                 "Outros jogadores <u> não verão as mudanças </u> a menos que você distribua seus arquivos de dados para eles.<br><br>" +
                                                 "Esta operação não pode ser concluída a menos que o servidor e o cliente estejam usando arquivos de dados diferentes.  " +
                                                 "Se você receber uma mensagem dizendo 'os arquivos de dados de saída não puderam ser abertos', provavelmente você está compartilhando arquivos de dados.  " +
                                                 "Crie um novo diretório para os arquivos de dados mundiais (statics * .mul e staidx * .mul) e adicione-o a Scritps / Misc / DataPath.cs.<br><br>" +
                                                 "A alteração entrará em vigor imediatamente no servidor, no entanto, você deve reiniciar seu cliente e atualizar seus arquivos de dados para que as alterações se tornem visíveis.  " +
                                                 "É altamente recomendável que você faça backup dos arquivos de dados mencionados acima.  " +
                                                 "Você deseja continuar?";
        private const string BaseUnfreezeWarning = "{0}  " +
                                                   "Esses itens <u> serão removidos dos arquivos estáticos </u> e trocados por itens dinâmicos não movíveis.  " +
                                                   "Outros jogadores <u> não verão as mudanças </u> a menos que você distribua seus arquivos de dados para eles.<br><br>" +
                                                   "Esta operação não pode ser concluída a menos que o servidor e o cliente estejam usando arquivos de dados diferentes.  " +
                                                   "Se você receber uma mensagem informando 'não foi possível abrir os arquivos de dados de saída', provavelmente você está compartilhando arquivos de dados.  " +
                                                   "Crie um novo diretório para os arquivos de dados mundiais (statics * .mul e staidx * .mul) e adicione-o a Scritps / Misc / DataPath.cs.<br><br>" +
                                                   "A alteração entrará em vigor imediatamente no servidor, no entanto, você deve reiniciar seu cliente e atualizar seus arquivos de dados para que as alterações se tornem visíveis.  " +
                                                   "É altamente recomendável que você faça backup dos arquivos de dados mencionados acima. " +
                                                   "Do você deseja prosseguir?";
        public static void Initialize()
        {
            CommandSystem.Register("Freeze", AccessLevel.Administrator, new CommandEventHandler(Freeze_OnCommand));
            CommandSystem.Register("FreezeMap", AccessLevel.Administrator, new CommandEventHandler(FreezeMap_OnCommand));
            CommandSystem.Register("FreezeWorld", AccessLevel.Administrator, new CommandEventHandler(FreezeWorld_OnCommand));

            CommandSystem.Register("Unfreeze", AccessLevel.Administrator, new CommandEventHandler(Unfreeze_OnCommand));
            CommandSystem.Register("UnfreezeMap", AccessLevel.Administrator, new CommandEventHandler(UnfreezeMap_OnCommand));
            CommandSystem.Register("UnfreezeWorld", AccessLevel.Administrator, new CommandEventHandler(UnfreezeWorld_OnCommand));
        }

        [Usage("Freeze")]
        [Description("Torna estática uma área de destino de itens dinâmicos.")]
        public static void Freeze_OnCommand(CommandEventArgs e)
        {
            BoundingBoxPicker.Begin(e.Mobile, new BoundingBoxCallback(FreezeBox_Callback), null);
        }

        [Usage("FreezeMap")]
        [Description("Torna todos os itens dinâmicos em seu mapa estáticos.")]
        public static void FreezeMap_OnCommand(CommandEventArgs e)
        {
            Map map = e.Mobile.Map;

            if (map != null && map != Map.Internal)
                SendWarning(e.Mobile, "Você está prestes a congelar <u> todos os itens em {0}</u>.", BaseFreezeWarning, map, NullP3D, NullP3D, new WarningGumpCallback(FreezeWarning_Callback));
        }

        [Usage("FreezeWorld")]
        [Description("Torna todos os itens dinâmicos em todos os mapas estáticos.")]
        public static void FreezeWorld_OnCommand(CommandEventArgs e)
        {
            SendWarning(e.Mobile, "Você está prestes a congelar <u> todos os itens em todos os mapas</u>.", BaseFreezeWarning, null, NullP3D, NullP3D, new WarningGumpCallback(FreezeWarning_Callback));
        }

        public static void SendWarning(Mobile m, string header, string baseWarning, Map map, Point3D start, Point3D end, WarningGumpCallback callback)
        {
            m.SendGump(new WarningGump(1060635, 30720, String.Format(baseWarning, String.Format(header, map)), 0xFFC000, 420, 400, callback, new StateInfo(map, start, end)));
        }

        public static void Freeze(Mobile from, Map targetMap, Point3D start3d, Point3D end3d)
        {
            Hashtable mapTable = new Hashtable();

            if (start3d == NullP3D && end3d == NullP3D)
            {
                if (targetMap == null)
                    CommandLogging.WriteLine(from, "{0} {1} invocando congelamento para cada item em cada mapa", from.AccessLevel, CommandLogging.Format(from));
                else
                    CommandLogging.WriteLine(from, "{0} {1} invocando congelamento para cada item em{0}", from.AccessLevel, CommandLogging.Format(from), targetMap);

                foreach (Item item in World.Items.Values)
                {
                    if (targetMap != null && item.Map != targetMap)
                        continue;

                    if (item.Parent != null)
                        continue;

                    if (item is Static || item is BaseFloor || item is BaseWall)
                    {
                        Map itemMap = item.Map;

                        if (itemMap == null || itemMap == Map.Internal)
                            continue;

                        Hashtable table = (Hashtable)mapTable[itemMap];

                        if (table == null)
                            mapTable[itemMap] = table = new Hashtable();

                        Point2D p = new Point2D(item.X >> 3, item.Y >> 3);

                        DeltaState state = (DeltaState)table[p];

                        if (state == null)
                            table[p] = state = new DeltaState(p);

                        state.m_List.Add(item);
                    }
                }
            }
            else if (targetMap != null)
            {
                Point2D start = targetMap.Bound(new Point2D(start3d)), end = targetMap.Bound(new Point2D(end3d));

                CommandLogging.WriteLine(from, "{0} {1} invocando congelamento de {2} to {3} in {4}", from.AccessLevel, CommandLogging.Format(from), start, end, targetMap);

                IPooledEnumerable eable = targetMap.GetItemsInBounds(new Rectangle2D(start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1));

                foreach (Item item in eable)
                {
                    if (item is Static || item is BaseFloor || item is BaseWall)
                    {
                        Map itemMap = item.Map;

                        if (itemMap == null || itemMap == Map.Internal)
                            continue;

                        Hashtable table = (Hashtable)mapTable[itemMap];

                        if (table == null)
                            mapTable[itemMap] = table = new Hashtable();

                        Point2D p = new Point2D(item.X >> 3, item.Y >> 3);

                        DeltaState state = (DeltaState)table[p];

                        if (state == null)
                            table[p] = state = new DeltaState(p);

                        state.m_List.Add(item);
                    }
                }

                eable.Free();
            }

            if (mapTable.Count == 0)
            {
                from.SendGump(new NoticeGump(1060637, 30720, "Nenhum item congelável foi encontrado. Apenas os seguintes tipos de itens são congelados:<br> - Static<br> - BaseFloor<br> - BaseWall", 0xFFC000, 320, 240, null, null));
                return;
            }

            bool badDataFile = false;

            int totalFrozen = 0;

            foreach (DictionaryEntry de in mapTable)
            {
                Map map = (Map)de.Key;
                Hashtable table = (Hashtable)de.Value;

                TileMatrix matrix = map.Tiles;

                using (FileStream idxStream = OpenWrite(matrix.IndexStream))
                {
                    using (FileStream mulStream = OpenWrite(matrix.DataStream))
                    {
                        if (idxStream == null || mulStream == null)
                        {
                            badDataFile = true;
                            continue;
                        }

                        BinaryReader idxReader = new BinaryReader(idxStream);

                        BinaryWriter idxWriter = new BinaryWriter(idxStream);
                        BinaryWriter mulWriter = new BinaryWriter(mulStream);

                        foreach (DeltaState state in table.Values)
                        {
                            int oldTileCount;
                            StaticTile[] oldTiles = ReadStaticBlock(idxReader, mulStream, state.m_X, state.m_Y, matrix.BlockWidth, matrix.BlockHeight, out oldTileCount);

                            if (oldTileCount < 0)
                                continue;

                            int newTileCount = 0;
                            StaticTile[] newTiles = new StaticTile[state.m_List.Count];

                            for (int i = 0; i < state.m_List.Count; ++i)
                            {
                                Item item = state.m_List[i];

                                int xOffset = item.X - (state.m_X * 8);
                                int yOffset = item.Y - (state.m_Y * 8);

                                if (xOffset < 0 || xOffset >= 8 || yOffset < 0 || yOffset >= 8)
                                    continue;

                                StaticTile newTile = new StaticTile((ushort)item.ItemID, (byte)xOffset, (byte)yOffset, (sbyte)item.Z, (short)item.Hue);

                                newTiles[newTileCount++] = newTile;

                                item.Delete();

                                ++totalFrozen;
                            }

                            int mulPos = -1;
                            int length = -1;
                            int extra = 0;

                            if ((oldTileCount + newTileCount) > 0)
                            {
                                mulWriter.Seek(0, SeekOrigin.End);

                                mulPos = (int)mulWriter.BaseStream.Position;
                                length = (oldTileCount + newTileCount) * 7;
                                extra = 1;

                                for (int i = 0; i < oldTileCount; ++i)
                                {
                                    StaticTile toWrite = oldTiles[i];

                                    mulWriter.Write((ushort)toWrite.ID);
                                    mulWriter.Write((byte)toWrite.X);
                                    mulWriter.Write((byte)toWrite.Y);
                                    mulWriter.Write((sbyte)toWrite.Z);
                                    mulWriter.Write((short)toWrite.Hue);
                                }

                                for (int i = 0; i < newTileCount; ++i)
                                {
                                    StaticTile toWrite = newTiles[i];

                                    mulWriter.Write((ushort)toWrite.ID);
                                    mulWriter.Write((byte)toWrite.X);
                                    mulWriter.Write((byte)toWrite.Y);
                                    mulWriter.Write((sbyte)toWrite.Z);
                                    mulWriter.Write((short)toWrite.Hue);
                                }

                                mulWriter.Flush();
                            }

                            int idxPos = ((state.m_X * matrix.BlockHeight) + state.m_Y) * 12;

                            idxWriter.Seek(idxPos, SeekOrigin.Begin);
                            idxWriter.Write(mulPos);
                            idxWriter.Write(length);
                            idxWriter.Write(extra);

                            idxWriter.Flush();

                            matrix.SetStaticBlock(state.m_X, state.m_Y, null);
                        }
                    }
                }
            }

            if (totalFrozen == 0 && badDataFile)
                from.SendGump(new NoticeGump(1060637, 30720, "Os arquivos de dados de saída não puderam ser abertos e a operação de congelamento foi abortada. <br> <br> Isso provavelmente significa que seu servidor e cliente estão usando os mesmos arquivos de dados. As instruções sobre como resolver isso podem ser encontradas na primeira janela de aviso.", 0xFFC000, 320, 240, null, null));
            else
                from.SendGump(new NoticeGump(1060637, 30720, String.Format("Operação de congelamento concluída com sucesso. <br> <br> {0} item {1} congelado. <br> <br> Você deve reiniciar seu cliente e atualizar seus arquivos de dados para ver as alterações.", totalFrozen, totalFrozen != 1 ? "s were" : " was"), 0xFFC000, 320, 240, null, null));
        }

        [Usage("Unfreeze")]
        [Description("Torna uma área de destino de itens estáticos dinâmica.")]
        public static void Unfreeze_OnCommand(CommandEventArgs e)
        {
            BoundingBoxPicker.Begin(e.Mobile, new BoundingBoxCallback(UnfreezeBox_Callback), null);
        }

        [Usage("UnfreezeMap")]
        [Description("Torna cada item estático em seu mapa dinâmico.")]
        public static void UnfreezeMap_OnCommand(CommandEventArgs e)
        {
            Map map = e.Mobile.Map;

            if (map != null && map != Map.Internal)
                SendWarning(e.Mobile, "Você está prestes a descongelar <u> todos os itens em {0}</u>.", BaseUnfreezeWarning, map, NullP3D, NullP3D, new WarningGumpCallback(UnfreezeWarning_Callback));
        }

        [Usage("UnfreezeWorld")]
        [Description("Torna todos os itens estáticos em todos os mapas dinâmicos.")]
        public static void UnfreezeWorld_OnCommand(CommandEventArgs e)
        {
            SendWarning(e.Mobile, "Você está prestes a descongelar <u> todos os itens em todos os mapas</u>.", BaseUnfreezeWarning, null, NullP3D, NullP3D, new WarningGumpCallback(UnfreezeWarning_Callback));
        }

        public static void DoUnfreeze(Map map, ref bool badDataFile, ref int totalUnfrozen)
        {
            DoUnfreeze(map, Point2D.Zero, new Point2D(map.Width - 1, map.Height - 1), ref badDataFile, ref totalUnfrozen);
        }

        public static void Unfreeze(Mobile from, Map map, Point3D start, Point3D end)
        {
            int totalUnfrozen = 0;
            bool badDataFile = false;

            if (map == null)
            {
                CommandLogging.WriteLine(from, "{0} {1} invocando o descongelamento para cada item em cada mapa", from.AccessLevel, CommandLogging.Format(from));

                DoUnfreeze(Map.Felucca, ref badDataFile, ref totalUnfrozen);
                DoUnfreeze(Map.Trammel, ref badDataFile, ref totalUnfrozen);
                DoUnfreeze(Map.Ilshenar, ref badDataFile, ref totalUnfrozen);
                DoUnfreeze(Map.Malas, ref badDataFile, ref totalUnfrozen);
                DoUnfreeze(Map.Tokuno, ref badDataFile, ref totalUnfrozen);
            }
            else if (start == NullP3D && end == NullP3D)
            {
                CommandLogging.WriteLine(from, "{0} {1} invocando o descongelamento para cada item em{2}", from.AccessLevel, CommandLogging.Format(from), map);

                DoUnfreeze(map, ref badDataFile, ref totalUnfrozen);
            }
            else
            {
                CommandLogging.WriteLine(from, "{0} {1} invocando o descongelamento de {2} to {3} in {4}", from.AccessLevel, CommandLogging.Format(from), new Point2D(start), new Point2D(end), map);

                DoUnfreeze(map, new Point2D(start), new Point2D(end), ref badDataFile, ref totalUnfrozen);
            }

            if (totalUnfrozen == 0 && badDataFile)
                from.SendGump(new NoticeGump(1060637, 30720, "Os arquivos de dados de saída não puderam ser abertos e a operação de descongelamento foi abortada. <br> <br> Isso provavelmente significa que seu servidor e cliente estão usando os mesmos arquivos de dados. As instruções sobre como resolver isso podem ser encontradas na primeira janela de aviso.", 0xFFC000, 320, 240, null, null));
            else
                from.SendGump(new NoticeGump(1060637, 30720, String.Format("Operação de descongelamento concluída com sucesso. <br> <br> {0} item {1} descongelado. <br> <br> Você deve reiniciar seu cliente e atualizar seus arquivos de dados para ver as alterações.", totalUnfrozen, totalUnfrozen != 1 ? "s were" : " was"), 0xFFC000, 320, 240, null, null));
        }

        private static void FreezeBox_Callback(Mobile from, Map map, Point3D start, Point3D end, object state)
        {
            SendWarning(from, "Você está prestes a congelar uma seção de itens.", BaseFreezeWarning, map, start, end, new WarningGumpCallback(FreezeWarning_Callback));
        }

        private static void FreezeWarning_Callback(Mobile from, bool okay, object state)
        {
            if (!okay)
                return;

            StateInfo si = (StateInfo)state;

            Freeze(from, si.m_Map, si.m_Start, si.m_End);
        }

        private static void UnfreezeBox_Callback(Mobile from, Map map, Point3D start, Point3D end, object state)
        {
            SendWarning(from, "Você está prestes a descongelar uma seção de itens.", BaseUnfreezeWarning, map, start, end, new WarningGumpCallback(UnfreezeWarning_Callback));
        }

        private static void UnfreezeWarning_Callback(Mobile from, bool okay, object state)
        {
            if (!okay)
                return;

            StateInfo si = (StateInfo)state;

            Unfreeze(from, si.m_Map, si.m_Start, si.m_End);
        }

        private static void DoUnfreeze(Map map, Point2D start, Point2D end, ref bool badDataFile, ref int totalUnfrozen)
        {
            start = map.Bound(start);
            end = map.Bound(end);

            int xStartBlock = start.X >> 3;
            int yStartBlock = start.Y >> 3;
            int xEndBlock = end.X >> 3;
            int yEndBlock = end.Y >> 3;

            int xTileStart = start.X, yTileStart = start.Y;
            int xTileWidth = end.X - start.X + 1, yTileHeight = end.Y - start.Y + 1;

            TileMatrix matrix = map.Tiles;

            using (FileStream idxStream = OpenWrite(matrix.IndexStream))
            {
                using (FileStream mulStream = OpenWrite(matrix.DataStream))
                {
                    if (idxStream == null || mulStream == null)
                    {
                        badDataFile = true;
                        return;
                    }

                    BinaryReader idxReader = new BinaryReader(idxStream);

                    BinaryWriter idxWriter = new BinaryWriter(idxStream);
                    BinaryWriter mulWriter = new BinaryWriter(mulStream);

                    for (int x = xStartBlock; x <= xEndBlock; ++x)
                    {
                        for (int y = yStartBlock; y <= yEndBlock; ++y)
                        {
                            int oldTileCount;
                            StaticTile[] oldTiles = ReadStaticBlock(idxReader, mulStream, x, y, matrix.BlockWidth, matrix.BlockHeight, out oldTileCount);

                            if (oldTileCount < 0)
                                continue;

                            int newTileCount = 0;
                            StaticTile[] newTiles = new StaticTile[oldTileCount];

                            int baseX = (x << 3) - xTileStart, baseY = (y << 3) - yTileStart;

                            for (int i = 0; i < oldTileCount; ++i)
                            {
                                StaticTile oldTile = oldTiles[i];

                                int px = baseX + oldTile.X;
                                int py = baseY + oldTile.Y;

                                if (px < 0 || px >= xTileWidth || py < 0 || py >= yTileHeight)
                                {
                                    newTiles[newTileCount++] = oldTile;
                                }
                                else
                                {
                                    ++totalUnfrozen;

                                    Item item = new Static(oldTile.ID);

                                    item.Hue = oldTile.Hue;

                                    item.MoveToWorld(new Point3D(px + xTileStart, py + yTileStart, oldTile.Z), map);
                                }
                            }

                            int mulPos = -1;
                            int length = -1;
                            int extra = 0;

                            if (newTileCount > 0)
                            {
                                mulWriter.Seek(0, SeekOrigin.End);

                                mulPos = (int)mulWriter.BaseStream.Position;
                                length = newTileCount * 7;
                                extra = 1;

                                for (int i = 0; i < newTileCount; ++i)
                                {
                                    StaticTile toWrite = newTiles[i];

                                    mulWriter.Write((ushort)toWrite.ID);
                                    mulWriter.Write((byte)toWrite.X);
                                    mulWriter.Write((byte)toWrite.Y);
                                    mulWriter.Write((sbyte)toWrite.Z);
                                    mulWriter.Write((short)toWrite.Hue);
                                }

                                mulWriter.Flush();
                            }

                            int idxPos = ((x * matrix.BlockHeight) + y) * 12;

                            idxWriter.Seek(idxPos, SeekOrigin.Begin);
                            idxWriter.Write(mulPos);
                            idxWriter.Write(length);
                            idxWriter.Write(extra);

                            idxWriter.Flush();

                            matrix.SetStaticBlock(x, y, null);
                        }
                    }
                }
            }
        }

        private static FileStream OpenWrite(FileStream orig)
        {
            if (orig == null)
                return null;

            try
            {
                return new FileStream(orig.Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch
            {
                return null;
            }
        }

        private static StaticTile[] ReadStaticBlock(BinaryReader idxReader, FileStream mulStream, int x, int y, int width, int height, out int count)
        {
            try
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    count = -1;
                    return m_TileBuffer;
                }

                idxReader.BaseStream.Seek(((x * height) + y) * 12, SeekOrigin.Begin);

                int lookup = idxReader.ReadInt32();
                int length = idxReader.ReadInt32();

                if (lookup < 0 || length <= 0)
                {
                    count = 0;
                }
                else
                {
                    count = length / 7;

                    mulStream.Seek(lookup, SeekOrigin.Begin);

                    if (m_TileBuffer.Length < count)
                        m_TileBuffer = new StaticTile[count];

                    StaticTile[] staTiles = m_TileBuffer;

                    if (m_Buffer == null || length > m_Buffer.Length)
                        m_Buffer = new byte[length];

                    mulStream.Read(m_Buffer, 0, length);

                    int index = 0;

                    for (int i = 0; i < count; ++i)
                    {
                        staTiles[i].Set((ushort)(m_Buffer[index++] | (m_Buffer[index++] << 8)),
                            (byte)m_Buffer[index++], (byte)m_Buffer[index++], (sbyte)m_Buffer[index++],
                            (short)(m_Buffer[index++] | (m_Buffer[index++] << 8)));
                    }
                }
            }
            catch
            {
                count = -1;
            }

            return m_TileBuffer;
        }

        private class DeltaState
        {
            public readonly int m_X;
            public readonly int m_Y;
            public readonly List<Item> m_List;
            public DeltaState(Point2D p)
            {
                this.m_X = p.X;
                this.m_Y = p.Y;
                this.m_List = new List<Item>();
            }
        }

        private class StateInfo
        {
            public readonly Map m_Map;
            public readonly Point3D m_Start;
            public readonly Point3D m_End;
            public StateInfo(Map map, Point3D start, Point3D end)
            {
                this.m_Map = map;
                this.m_Start = start;
                this.m_End = end;
            }
        }
    }
}
