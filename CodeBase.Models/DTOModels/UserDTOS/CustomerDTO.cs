﻿namespace CodeBase.Models.DTOModels.UserDTOS;
public class CustomerDTO
{
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
