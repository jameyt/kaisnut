﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public interface IContact
    {
        int Id { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Address { get; set; }
    }
}
