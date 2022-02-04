using Microsoft.Win32;
using Server;
using System;
using System.IO;

public class DataPathServer
{
    /* If you have not installed Ultima Online,
    * or wish the server to use a separate set of datafiles,
    * change the 'CustomPath' value.
    * Example:
    *  private static string CustomPath = @"C:\Program Files\Ultima Online";
    */
    private static readonly string CustomPath = Config.Get(@"DataPath.CustomPath", default(string));
    public static bool Loaded = false;

    public static void ReloadPath()
    {
       
        if (Loaded)
        {
            return;
        }
        Console.WriteLine("Reload Path Server");
        string path = "";

        Console.WriteLine("=========== PATHS ===========");

        if (CustomPath != null)
        {
            Console.WriteLine("CUSTOM PATH: " + CustomPath);
            path = CustomPath;
        }
        else if (!Core.Unix)
        {
            Console.WriteLine("Path Windaum");
            //path = Files.LoadDirectory();
        }
        else
        {
            Console.WriteLine("Path Cagada");
            if(Core.Unix)
            {
                Console.WriteLine("Setando path LINUX /uo");
                path = "/uo";
            } else
            {
                path = null;
            }
        }

        //Console.WriteLine("-- PATH: " + path);
        if (!String.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Carregando arquivos da path");
            Core.DataDirectories.Add(path);
        }
        else
        {
            throw new Exception("Nao carregou path do UO " + path);
        }
        Loaded = true;
    }

    static DataPathServer()
    {
        ReloadPath();
    }

    /* The following is a list of files which a required for proper execution:
    * 
    * Multi.idx
    * Multi.mul
    * VerData.mul
    * TileData.mul
    * Map*.mul or Map*LegacyMUL.uop
    * StaIdx*.mul
    * Statics*.mul
    * MapDif*.mul
    * MapDifL*.mul
    * StaDif*.mul
    * StaDifL*.mul
    * StaDifI*.mul
    */
    public static void Configure()
    {
        if (CustomPath != null)
        {
            Core.DataDirectories.Add(CustomPath);
        }
        else
        {
            Console.WriteLine("DATA PATH CAGOU");
        }
    }

    private static string GetPath(string subName, string keyName)
    {
        try
        {
            string keyString;

            if (Core.Is64Bit)
                keyString = @"SOFTWARE\Wow6432Node\{0}";
            else
                keyString = @"SOFTWARE\{0}";

            using (var key = Registry.LocalMachine.OpenSubKey(String.Format(keyString, subName)))
            {
                if (key == null)
                    return null;

                var v = key.GetValue(keyName) as string;

                if (String.IsNullOrEmpty(v))
                    return null;

                if (keyName == "InstallDir")
                    v = v + @"\";

                v = Path.GetDirectoryName(v);

                if (String.IsNullOrEmpty(v))
                    return null;

                return v;
            }
        }
        catch
        {
            return null;
        }
    }
}
