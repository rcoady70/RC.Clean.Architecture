﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Domain.Entities.CSV;

public enum FileStatus
{
    Failed = -10,
    NotSet = 0,
    Uploaded = 10,
    OnQueue = 20,
    BeingProcessed = 30,
    Finished = 40
}
