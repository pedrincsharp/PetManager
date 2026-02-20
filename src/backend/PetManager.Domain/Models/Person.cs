using System;
using PetManager.Domain.Models.Enums;

namespace PetManager.Domain.Models;

public class Person: ModelBase
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Cellphone { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public Role Role { get; private set; }

    protected Person(){}
    public Person(string name, string email, string cellphone, string document, Role role)
    {
        Name = name;
        Email = email;
        Cellphone = cellphone;
        Document = document;
        Role = role;
    }

    public void ChangeName(string name) => Name = name;
    public void ChangeEmail(string email) => Email = email;
    public void ChangeCellphone(string cellphone) => Cellphone = cellphone;
    public void ChangeDocument(string document) => Document = document;
    public void ChangeRole(Role role) => Role = role;
}
