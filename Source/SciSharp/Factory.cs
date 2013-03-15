using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;


namespace SciSharp
{
    public class Factory<T> : IFactory<T>
    {
        #region Instance Fields

        private readonly Func<T> creation;

        #endregion

        #region Constructors

        public Factory()
            : this(Activator.CreateInstance<T>) {}

        public Factory(Func<T> creation)
        {
            if (creation == null)
                throw new ArgumentNullException("creation");

            this.creation = creation;
        }

        #endregion

        #region IFactory<T> Members

        public T Create()
        {
            return creation();
        }

        #endregion
    }

    public static class Factory
    {
        public static dynamic New
        {
            get { return new ProxyFactory(); }
        }

        #region Nested type: Proxy

        private class Proxy : DynamicObject
        {
            private readonly Dictionary<string, object> members;

            public Proxy(Dictionary<string, object> members)
            {
                if (members == null)
                    throw new ArgumentNullException("members");

                this.members = members;
            }

            public override bool TryConvert(ConvertBinder binder, out object result)
            {
                Type type = binder.Type;

                if (!type.IsInterface)
                    throw new InvalidOperationException("Cannot convert to a non-interface type.");

                var typeDeclaration = new CodeTypeDeclaration();
                typeDeclaration.BaseTypes.Add(type);

                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    typeDeclaration.Members.Add(new CodeMemberProperty
                                                {
                                                    Name = propertyInfo.Name,
                                                    Type = new CodeTypeReference(propertyInfo.PropertyType),
                                                    GetStatements =
                                                    {
                                                        new CodeMethodReturnStatement(new CodePrimitiveExpression(members[propertyInfo.Name]))
                                                    }
                                                });
                }

                result = null;
                return true;
            }
        }

        #endregion

        #region Nested type: ProxyFactory

        private class ProxyFactory : DynamicObject
        {
            public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
            {
                var members = new Dictionary<string, object>();

                foreach (var memberInfo in binder.CallInfo.ArgumentNames.Zip(args, Tuple.Create))
                    members[memberInfo.Item1] = memberInfo.Item2;

                result = new Proxy(members);

                return true;
            }
        }

        #endregion
    }
}
