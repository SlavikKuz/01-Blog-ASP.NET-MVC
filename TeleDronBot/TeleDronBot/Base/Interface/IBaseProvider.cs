﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TeleDronBot.Interfaces
{
    interface IBaseProvider<T> where T : class
    {
        Task Update(T item);
        Task Delete(T item);
        Task Create(T item);

        ValueTask<T> FirstElement(Func<T, bool> predicate);
        ValueTask<T> LastElement(Func<T, bool> predicate);
        ValueTask<T> FindById(long id);
        ValueTask<IEnumerable<T>> Get();
        ValueTask<IEnumerable<T>> Get(Func<T, bool> predicate);
    }
}