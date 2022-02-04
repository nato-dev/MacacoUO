#region References
using System;
using System.IO;
using System.Text;

using Server.Network;
#endregion

namespace Server
{
	public sealed class ObjectPropertyList : Packet
	{
		private readonly IEntity m_Entity;
		private int m_Hash;
		private int m_Header;
		private int m_Strings;
        private int m_Strings2;
        private int m_Strings3;
        private string m_HeaderArgs;

		public IEntity Entity { get { return m_Entity; } }
		public int Hash { get { return 0x40000000 + m_Hash; } }

		public int Header { get { return m_Header; } set { m_Header = value; } }
		public string HeaderArgs { get { return m_HeaderArgs; } set { m_HeaderArgs = value; } }

		public static bool Enabled { get; set; }

		public ObjectPropertyList(IEntity e)
			: base(0xD6)
		{
			EnsureCapacity(128);

			m_Entity = e;

			m_Stream.Write((short)1);
			m_Stream.Write(e.Serial);
			m_Stream.Write((byte)0);
			m_Stream.Write((byte)0);
			m_Stream.Write(e.Serial);
		}

		public void Add(int number)
		{
			if (number == 0)
			{
				return;
			}

			AddHash(number);

			if (m_Header == 0)
			{
				m_Header = number;
				m_HeaderArgs = "";
			}

			m_Stream.Write(number);
			m_Stream.Write((short)0);
		}

		public void Terminate()
		{
			m_Stream.Write(0);

			m_Stream.Seek(11, SeekOrigin.Begin);
			m_Stream.Write(m_Hash);
		}

		private static byte[] m_Buffer = new byte[1024];
		private static readonly Encoding m_Encoding = Encoding.Unicode;

		public void AddHash(int val)
		{
			m_Hash ^= (val & 0x3FFFFFF);
			m_Hash ^= (val >> 26) & 0x3F;
		}

		public void Add(int number, string arguments)
		{
			if (number == 0)
			{
				return;
			}

			if (arguments == null)
			{
				arguments = "";
			}

			if (m_Header == 0)
			{
				m_Header = number;
				m_HeaderArgs = arguments;
			}

			AddHash(number);
			AddHash(arguments.GetHashCode());

			m_Stream.Write(number);

			int byteCount = m_Encoding.GetByteCount(arguments);

			if (byteCount > m_Buffer.Length)
			{
				m_Buffer = new byte[byteCount];
			}

			byteCount = m_Encoding.GetBytes(arguments, 0, arguments.Length, m_Buffer, 0);

			m_Stream.Write((short)byteCount);
			m_Stream.Write(m_Buffer, 0, byteCount);
		}

        public void AddTwoValues(string s1, string s2)
        {
            var id = m_StringNumbers_2_values[m_Strings2++ % m_StringNumbers_2_values.Length];
            Add(id, s1+"\t{0}", s2);
        }

        public void AddTwoValues(string s1, int s2)
        {
            AddTwoValues(s1, s2.ToString());
        }

        public void AddThreeValues(string s1, string s2, string s3)
        {
            var id = m_StringNumbers_3_values[m_Strings3++ % m_StringNumbers_3_values.Length];
            Add(id, "#{0}\t#{1}\t{2}", s1, s2, s3);
        }

        public void Add(int number, string format, object arg0)
		{
			Add(number, String.Format(format, arg0));
		}

		public void Add(int number, string format, object arg0, object arg1)
		{
			Add(number, String.Format(format, arg0, arg1));
		}

		public void Add(int number, string format, object arg0, object arg1, object arg2)
		{
			Add(number, String.Format(format, arg0, arg1, arg2));
		}

		public void Add(int number, string format, params object[] args)
		{
			Add(number, String.Format(format, args));
		}

		// Each of these are localized to "~1_NOTHING~" which allows the string argument to be used
		private static readonly int[] m_StringNumbers = new[] {
            1114057, 1114778, 1114779, // ~1_val~
			1150541, // ~1_TOKEN~
			1153153, // ~1_year~

			1041522, // ~1~~2~~3~
            1060658, //  ~1_val~ ~2_val~
            1060659, //  ~1_val~ ~2_val~
            1060847, // ~1_val~ ~2_val~
			1116560, // ~1_val~ ~2_val~
			1116690, // ~1_val~ ~2_val~ ~3_val~
			1116691, // ~1_val~ ~2_val~ ~3_val~
			1116692, // ~1_val~ ~2_val~ ~3_val~
			1116693, // ~1_val~ ~2_val~ ~3_val~
			1116694 // ~1_val~ ~2_val~ ~3_val~
        };

        private static readonly int[] m_StringNumbers_2_values = new[] {
            1060658, //  ~1_val~ ~2_val~
            1060659, //  ~1_val~ ~2_val~
            1060847, // ~1_val~ ~2_val~
			1116560, // ~1_val~ ~2_val~
        };

        private static readonly int[] m_StringNumbers_3_values = new[] {
            1116690, // ~1_val~ ~2_val~ ~3_val~
			1116691, // ~1_val~ ~2_val~ ~3_val~
			1116692, // ~1_val~ ~2_val~ ~3_val~
			1116693, // ~1_val~ ~2_val~ ~3_val~
			1116694  // ~1_val~ ~2_val~ ~3_val~
        };

        private int GetStringNumber()
		{
            //return 1114057;
            return m_StringNumbers[m_Strings++ % m_StringNumbers.Length];
        }

		public void Add(string text)
		{
			Add(GetStringNumber(), text);
		}

        public void Add(string format, string arg0)
		{
			Add(GetStringNumber(), String.Format(format, arg0));
		}

		public void Add(string format, string arg0, string arg1)
		{
			Add(GetStringNumber(), String.Format(format, arg0, arg1));
		}

		public void Add(string format, string arg0, string arg1, string arg2)
		{
			Add(GetStringNumber(), String.Format(format, arg0, arg1, arg2));
		}

		public void Add(string format, params object[] args)
		{
			Add(GetStringNumber(), String.Format(format, args));
		}
	}

	public sealed class OPLInfo : Packet
	{
		/*public OPLInfo( ObjectPropertyList list ) : base( 0xBF )
		{
			EnsureCapacity( 13 );

			m_Stream.Write( (short) 0x10 );
			m_Stream.Write( (int) list.Entity.Serial );
			m_Stream.Write( (int) list.Hash );
		}*/

		public OPLInfo(ObjectPropertyList list)
			: base(0xDC, 9)
		{
			m_Stream.Write(list.Entity.Serial);
			m_Stream.Write(list.Hash);
		}
	}
}
