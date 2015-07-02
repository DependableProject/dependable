using System.Threading.Tasks;

namespace Molecules.Core
{
    public class InvocableAtom<T> : Atom<T>
    {
        public Atom<T> Target { get; }

        public InvocableAtom(Atom<T> target)
        {
            Target = target;
        }

        public async Task<T> Charge()
        {
            return await Target.ChargeCore();
        }

        protected override Task<T> OnCharge(object input = null)
        {
            throw new System.NotImplementedException();
        }
    }

    public static partial class Atom
    {
        public static InvocableAtom<T> AsInvocable<T>(this Atom<T> target)
        {
            return new InvocableAtom<T>(target);
        }
    }
}