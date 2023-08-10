using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.TypeSearch
{
    public interface ITypeSearcher
    {
        IList<Assembly> GetAssemblies();

        IEnumerable<Type> ClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        IEnumerable<Type> ClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> ClassesOfType<T>(bool onlyConcreteClasses = true);

    }
}
