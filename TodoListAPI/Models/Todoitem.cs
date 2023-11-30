using System;
using System.Collections.Generic;

namespace TodoListAPI.Models;

public partial class Todoitem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
