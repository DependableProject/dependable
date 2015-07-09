using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Molecules.Core
{
    public class ConditionAtom<TSource, TOut> : Atom<TOut>
    {
        public Atom<TSource> Source { get; }

        public Predicate<TSource> Condition { get; }

        public Atom<TOut> Truthy { get; }

        public Atom<TOut> Falsey { get; }

        public ConditionAtom(Atom<TSource> source,
            Predicate<TSource> predicate,
            Atom<TOut> truthy,
            Atom<TOut> falsey)
        {
            Source = source;
            Condition = predicate;
            Truthy = truthy;
            Falsey = falsey;
        }

        internal async override Task<TOut> ChargeCore(IAtomContext atomContext)
        {
            var i = await Source.ChargeCore(atomContext);
            var next = Condition(i) ? Truthy : Falsey;
            return await next.ChargeCore(AtomContext.For(i));
        }        
    }

    public static partial class Atom
    {
        public static Atom<TOut> If<TSource, TOut>(this Atom<TSource> source,
            Predicate<TSource> predicate,
            Atom<TOut> truthy,
            Atom<TOut> falsey)
        {
            return new ConditionAtom<TSource, TOut>(source, predicate, truthy, falsey);
        }

        public static Atom<TOut> If<TSource, TOut>(this Atom<TSource> source,
            Predicate<TSource> predicate,
            Func<IAtomContext<TSource>, TOut> truthy,
            Func<IAtomContext<TSource>, TOut> falsey)
        {
            return If(source, predicate, Func(truthy), Func(falsey));
        }

        public static Atom<TOut> If<TSource, TOut>(this Atom<TSource> source,
            Predicate<TSource> predicate,
            Func<TOut> truthy,
            Func<TOut> falsey)
        {
            return If(source, predicate, Func(truthy), Func(falsey));
        }
    }
}