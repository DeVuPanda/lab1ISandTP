using System;
using System.Collections.Generic;

namespace ArchiveDomain.Model;

public partial class User: Entity
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual SearchHistory UserNavigation { get; set; } = null!;

    public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
}
