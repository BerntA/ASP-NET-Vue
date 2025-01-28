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

    public async Task<Contact?> AddContact(CreateContact data)
    {
        if (string.IsNullOrEmpty(data.Name))
            return null;

        var existingContact = await _dbContext
            .Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => EF.Functions.Like(c.Name, $"%{data.Name}%"));

        if (existingContact is not null)
            return null;

        var contact = new Contact
        {
            Name = data.Name,
            Phone = data.Phone,
            Address = data.Address,
        };

        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact?> UpdateContact(UpdateContact data)
    {
        if (string.IsNullOrEmpty(data.Name))
            return null;

        var contact = await _dbContext.Contacts.FindAsync(data.Id);
        if (contact is null)
            return null;

        contact = data.Adapt(contact);
        await _dbContext.SaveChangesAsync();

        return contact;
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

    public async Task RemoveAll()
    {
        var items = await _dbContext.Contacts.ToListAsync();
        _dbContext.Contacts.RemoveRange(items);
        await _dbContext.SaveChangesAsync();
    }
}