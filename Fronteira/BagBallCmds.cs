using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Ziden
{
    #region References
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using global::Server.Commands;
    using global::Server.Targeting;

    #endregion

    namespace Server.Commands
    {

        public class InternalTarg : Target
        {

            public InternalTarg() : base(2, false, TargetFlags.None)
            {

            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                base.OnTarget(from, targeted);
            }
        }

        public class ActionCmd
        {
            public static void Initialize()
            {
                CommandSystem.Register("chute", AccessLevel.Player, Chute);
            }

            [Usage("Chute")]
            private static void Chute(CommandEventArgs e)
            {


            }
        }

        
    }

}
