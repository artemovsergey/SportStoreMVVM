using System;
using System.Collections.Generic;

namespace SportStoreMVVM.Models;

public partial class RelatedProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int RelatedProdutId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Product RelatedProdut { get; set; } = null!;
}
