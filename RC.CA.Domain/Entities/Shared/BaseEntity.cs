using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.Domain.Entities.Shared;
/// <summary>
/// Base entity interface. It is not generic on purpose uses object. Allows selection of entities from change tracker generically using IBaseEntity interface.
/// </summary>
public interface IBaseEntity
{
    object Id { get; set; }
    string? CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
    string? UpdatedBy { get; set; }
    DateTime UpdatedOn { get; set; }
}

/// <summary>
/// Base entity
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseEntity<T> : IBaseEntity
{
    public T? Id { get; set; }
    //Satisfy interface.
    //Interface to select entities from context change tracker generically.   
    object IBaseEntity.Id
    {
        get { return Id; }
        set { }
    }
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}
