using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities.Enums;
using Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities.Generated;

[PrimaryKey("UserId")]
public partial class Setting
{
    [Key]
    public string UserId { get; set; } = null!;
    public PreferredThemeEnum PreferredTheme {get;set;} = PreferredThemeEnum.Light;

}
