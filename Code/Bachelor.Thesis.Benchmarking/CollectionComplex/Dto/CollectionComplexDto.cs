﻿using System.ComponentModel.DataAnnotations;

namespace Bachelor.Thesis.Benchmarking.CollectionComplex.Dto;

public class CollectionComplexDto
{
    public static CollectionComplexDto ValidDto = new ()
    {
        ListOne = new ()
        {
            new (),
            new ()
        },
        ListTwo = new ()
        {
            new (),
            new (),
            new (),
            new (),
            new (),
            new (),
            new (),
            new (),
        }
    };

    public static CollectionComplexDto InvalidDto = new()
    {
        ListOne = new()
        {
            new(),
            new()
        },
        ListTwo = new()
        {
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new()
        }
    };

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<OrderDetails> ListOne { get; set; } = new ();

    [Required]
    [MinLength(1), MaxLength(10)]
    public List<Article> ListTwo { get; set; } = new ();
}