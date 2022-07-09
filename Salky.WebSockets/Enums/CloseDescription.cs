using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Enums
{
    public enum CloseDescription
    {
        Unknow = 0,
        Normal = 10,
        DuplicatedConnection = 20,
    }
}
