// Data/Repositories/NHibernateRepository.cs
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class NHibernateRepository<T> : IRepository<T> where T : class
{
    private readonly ISession _session;
    public NHibernateRepository(ISession session) => _session = session;

    public T Get(Guid id) => _session.Get<T>(id);

    public IEnumerable<T> GetAll() => _session.Query<T>().ToList();

    public void Add(T entity)
    {
        using var tx = _session.BeginTransaction();
        _session.Save(entity);
        tx.Commit();
    }

    public void Update(T entity)
    {
        using var tx = _session.BeginTransaction();
        _session.Update(entity);
        tx.Commit();
    }

    public void Delete(T entity)
    {
        using var tx = _session.BeginTransaction();
        _session.Delete(entity);
        tx.Commit();
    }
}
