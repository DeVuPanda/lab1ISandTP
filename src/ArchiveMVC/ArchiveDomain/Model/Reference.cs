using System;
using System.Collections.Generic;

namespace ArchiveDomain.Model;

public partial class Reference: Entity
{
    public int ReferenceId { get; set; }

    public string Description { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string Searchable { get; set; } = null!;

    public int SoId { get; set; }

    public virtual SearchObject So { get; set; } = null!;

    public virtual ICollection<Date> Dates { get; set; } = new List<Date>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
