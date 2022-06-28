using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Extensions
{
    public static class MaybeExtension
    {
        /// <summary>
        /// Can be used instead of ?. example person.WithExt(p => p.address).WithExt(p => p.Postcode)
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TResult WithExt<TInput, TResult>(this TInput self,Func<TInput,TResult> evaluate) 
            where TResult : class
            where TInput : class
        {
            if (self == null)
                return null;
            return evaluate(self);
        }
        /// <summary>
        ///  Check if person has a person.IfExt(HasMedicalRecord).WithExt(p => p.Postcode)
        ///  void HasMedicalRecord(Person person)
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static TInput IfExt<TInput>(this TInput self, Func<TInput,bool> evaluate)
            where TInput : class
        {
            if (self == null)
                return null;
            return evaluate(self) ? self :null;
        }
        /// <summary>
        /// Execute: example postcode = p.IfExt(HasMedicalRec).WithExt(p => p.Address).WithExt(p => p.Postcode).ExecExt(p => CheckAddress(p.Address))
        /// void CheckAddress(Address address)
        /// void HasMedicalRecord(Person person)
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="self"></param>
        /// <param name="evaluate"></param>
        /// <returns></returns>
        public static TInput ExecExt<TInput>(this TInput self, Action<TInput> action)
            where TInput: class
        {
            if (self == null)
                return null;
            action(self);
            return self;
        }
    }
}
