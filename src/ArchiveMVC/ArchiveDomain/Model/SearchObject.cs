using System;
using System.Collections.Generic;

namespace ArchiveDomain.Model;

public partial class SearchObject: Entity
{
    public int SoId { get; set; }

    public int ReferenceId { get; set; }

    public string FullName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Faculty { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly SearchTime { get; set; }

    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();

    public virtual ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
}
