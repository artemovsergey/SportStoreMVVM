using System;
using System.Collections.Generic;

namespace SportStoreMVVM.Models;

public partial class RelatedProduct
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int RelatedProductId { get; set; }



    public virtual Product RelatedProductNav { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;

    
}
