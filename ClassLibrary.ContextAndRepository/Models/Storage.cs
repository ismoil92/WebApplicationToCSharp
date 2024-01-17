﻿using ClassLibrary.ContextAndRepository.Models.Base;
namespace ClassLibrary.ContextAndRepository.Models;

public class Storage : BaseModel
{
    public int Count { get; set; }
    public virtual List<Product>? Products { get; set; }
}