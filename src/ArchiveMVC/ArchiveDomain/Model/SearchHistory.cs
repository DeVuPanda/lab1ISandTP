﻿using System;
using System.Collections.Generic;

namespace ArchiveDomain.Model;

public partial class SearchHistory: Entity
{
    public int UserId { get; set; }

    public int SearchSuccess { get; set; }

    public DateOnly SearchDate { get; set; }

    public int SoId { get; set; }

    public virtual SearchObject So { get; set; } = null!;

    public virtual User? User { get; set; }
}
