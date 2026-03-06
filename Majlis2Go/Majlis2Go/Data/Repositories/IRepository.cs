// Data/Repositories/IRepository.cs
using System;
using System.Collections.Generic;

public interface IRepository<T> where T : class
{
    T Get(Guid id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
