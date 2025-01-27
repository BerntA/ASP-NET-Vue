using database;
using database.Entities;
using lib.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace lib.Services;

public class ContactService : RepositoryService
{
    public ContactService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory) { }

    public async Task<List<Contact>> Contacts() => await _dbContext.Contacts.AsNoTracking().ToListAsync();

    public async Task<Guid> AddContact(CreateContact data)
    {
        var contact = new Contact
        {
            Name = data.Name,
            Phone = data.Phone,
            Address = data.Address,
        };

        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        return contact.Id;
    }

    public async Task<bool> UpdateContact(UpdateContact data)
    {
        var contact = await _dbContext.Contacts.FindAsync(data.Id);
        if (contact is null)
            return false;

        contact = data.Adapt(contact);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveContact(Guid id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);
        if (contact is null)
            return false;

        _dbContext.Remove(contact);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}