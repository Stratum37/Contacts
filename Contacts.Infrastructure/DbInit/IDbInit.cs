﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Infrastructure.DbInit
{
    public interface IDbInit
    {
        public void Initialize();
    }
}
