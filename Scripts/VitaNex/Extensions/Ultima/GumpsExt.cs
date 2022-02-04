#region Header
//   Vorspire    _,-'/-'/  GumpsExt.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2018  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion


#region References
using Server;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

using VitaNex;
#endregion

namespace Ultima
{
	public static class GumpsExtUtility
	{
		private static readonly MethodInfo _UltimaArtGetGump;

		static GumpsExtUtility()
		{
			try
			{
				var a = Assembly.LoadFrom("Ultima.dll");

				if (a == null)
				{
					return;
				}

				var t = a.GetType("Ultima.Gumps");

				if (t != null)
				{
					_UltimaArtGetGump = t.GetMethods(BindingFlags.Static | BindingFlags.Public)
										 .FirstOrDefault(
											 m => m.Name == "GetGump" && m.ReturnType.IsEqual<Bitmap>() && m.GetParameters().Length == 2);
				}
			}
			catch (Exception e)
			{
				VitaNexCore.ToConsole("Could not load Ultima.dll or a member of Ultima.Gumps:");
				VitaNexCore.ToConsole(e);
			}
		}

		private static Bitmap ExternalGetGump(int index, out bool patched)
		{
			var param = new object[] {index, false};

			var img = (Bitmap)_UltimaArtGetGump.Invoke(null, param);

			patched = (bool)param[1];

			return img;
		}

		// ReSharper disable UnusedParameter.Local
		private static Bitmap InternalGetGump(int index, out bool patched)
		{
            return GumpData.GetGump(index, out patched);
		}

		// ReSharper restore UnusedParameter.Local

		public static Bitmap GetGump(int index)
		{
			bool patched;
            
			return GetGump(index, out patched);
		}

		public static Bitmap GetGump(int index, out bool patched)
		{
            /*
			if (_UltimaArtGetGump != null)
			{
				return ExternalGetGump(index, out patched);
			}
            */
            Shard.Debug("Internal get gump");
			return InternalGetGump(index, out patched);
		}

		public static Size GetImageSize(int id)
		{
			var img = GetGump(id);

			if (img == null)
			{
				return new Size(0, 0);
			}

			return new Size(img.Width, img.Height);
		}

		public static int GetImageWidth(int id)
		{
			var img = GetGump(id);

			if (img == null)
			{
				return 0;
			}

			return img.Width;
		}

		public static int GetImageHeight(int id)
		{
			var img = GetGump(id);

			if (img == null)
			{
				return 0;
			}

			return img.Height;
		}
	}
}
