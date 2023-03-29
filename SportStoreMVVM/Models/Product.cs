using SportStoreMVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace SportStoreMVVM.Models;

public partial class Product : ViewModel
{
    public int? Id { get; set; }

    public string? ArticleNumber { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Cost { get; set; }

    public decimal? MaxDiscount { get; set; }

    public string Manufacturer { get; set; } = null!;

    public string Supplier { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal? DiscountAmount { get; set; }

    public int QuantityInStock { get; set; }

    public string Description { get; set; } = null!;

    public string Photo { get; set; } = null!;


    /*
     вы можете применить атрибут [NotMapped] к одному или нескольким свойствам,
     для которых вы НЕ хотите создавать соответствующий столбец в таблице базы данных.
     Этот атрибут применяется к EF 6 и EF core.
    */

    private string _imagePath;
    [NotMapped]
    public virtual string? ImagePath
    {

        get
        {
            if (File.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{Photo}")))
            {
                _imagePath = System.IO.Path.Combine(Environment.CurrentDirectory, $"images/{Photo}");
                return _imagePath;
            }
            else
            {
                Photo = "picture.png";
                return null;
            }
        }

        set
        {
            Set(ref _imagePath, value);
        }

    }


    public string Status { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual ICollection<RelatedProduct> RelatedProducts { get; } = new List<RelatedProduct>();

    public virtual ICollection<RelatedProduct> RelatedProductsNav { get; } = new List<RelatedProduct>();
}
