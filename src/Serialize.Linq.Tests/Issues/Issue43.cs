using System;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Serialize.Linq.Nodes;
using Serialize.Linq.Extensions;

namespace Serialize.Linq.Tests.Issues
{
    [TestClass]
    public class Issue43
    {
        private class User
        {
            public Guid Id
            {
                get;
                set;
            }
        }

        [TestMethod]
        public void ToExpressionNodeStackoverflow()
        {
            IList<Guid> idCollection = new List<Guid>();
            for (int i = 0; i < 5000; i++)
            {
                idCollection.Add(Guid.NewGuid());
            }

            var propertyInfo = typeof(User).GetProperty("Id");
            var userParam = Expression.Parameter(typeof(User), "x");
            var propertyExpression = Expression.Property(userParam, propertyInfo);
            Expression composedExpression = Expression.Constant(false, typeof(bool));

            for (int i = 0; i < idCollection.Count; i++)
            {
                Expression equalExpression = Expression.MakeBinary(ExpressionType.Equal, propertyExpression, Expression.Constant(idCollection[i]));
                composedExpression = Expression.MakeBinary(ExpressionType.OrElse, equalExpression, composedExpression);
            }
            var expressionNode = composedExpression.ToExpressionNode();
        }
    }
}
