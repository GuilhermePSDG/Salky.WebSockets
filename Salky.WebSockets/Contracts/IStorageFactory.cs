﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Contracts
{
    public interface IStorageFactory
    {
        public IStorage CreateNew();
    }
}
