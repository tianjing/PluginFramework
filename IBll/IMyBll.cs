﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IBLL
{
    public interface IMyBll
    {
        DataTable GetList();
        string hello();
    }
}
