﻿#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Serialize.Linq.Interfaces;
using Serialize.Linq.Nodes;

namespace Serialize.Linq.Factories
{
    public class NodeFactory : INodeFactory
    {
        /// <summary>
        /// Creates an expression node from an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown expression of type  + expression.GetType()</exception>
        public virtual ExpressionNode Create(Expression expression)
        {
            if (expression == null)
                return null;

            if (expression is BinaryExpression) return new BinaryExpressionNode(this, expression as BinaryExpression);
            if (expression is ConditionalExpression) return new ConditionalExpressionNode(this, expression as ConditionalExpression);
            if (expression is ConstantExpression) return new ConstantExpressionNode(this, expression as ConstantExpression);
            if (expression is InvocationExpression) return new InvocationExpressionNode(this, expression as InvocationExpression);
            if (expression is LambdaExpression) return new LambdaExpressionNode(this, expression as LambdaExpression);
            if (expression is ListInitExpression) return new ListInitExpressionNode(this, expression as ListInitExpression);
            if (expression is MemberExpression) return new MemberExpressionNode(this, expression as MemberExpression);
            if (expression is MemberInitExpression) return new MemberInitExpressionNode(this, expression as MemberInitExpression);
            if (expression is MethodCallExpression) return new MethodCallExpressionNode(this, expression as MethodCallExpression);
            if (expression is NewArrayExpression) return new NewArrayExpressionNode(this, expression as NewArrayExpression);
            if (expression is NewExpression) return new NewExpressionNode(this, expression as NewExpression);
            if (expression is ParameterExpression) return new ParameterExpressionNode(this, expression as ParameterExpression);
            if (expression is TypeBinaryExpression) return new TypeBinaryExpressionNode(this, expression as TypeBinaryExpression);
            if (expression is UnaryExpression) return new UnaryExpressionNode(this, expression as UnaryExpression);

            throw new ArgumentException("Unknown expression of type " + expression.GetType());
        }

        public async virtual Task<ExpressionNode> CreateAsync(Expression expression)
        {
            return await await Task.Factory.StartNew<Task<ExpressionNode>>( async () => 
            {
                if (expression == null)
                    return null;

                if (expression is BinaryExpression) return await BinaryExpressionNode.CreateAsync(this, expression as BinaryExpression);
                if (expression is ConditionalExpression) return await ConditionalExpressionNode.CreateAsync(this, expression as ConditionalExpression);
                if (expression is ConstantExpression) return new ConstantExpressionNode(this, expression as ConstantExpression);
                if (expression is InvocationExpression) return await InvocationExpressionNode.CreateAsync(this, expression as InvocationExpression);
                if (expression is LambdaExpression) return await LambdaExpressionNode.CreateAsync(this, expression as LambdaExpression);
                if (expression is ListInitExpression) return await ListInitExpressionNode.CreateAsync(this, expression as ListInitExpression);
                if (expression is MemberExpression) return await MemberExpressionNode.CreateAsync(this, expression as MemberExpression);
                if (expression is MemberInitExpression) return await MemberInitExpressionNode.CreateAsync(this, expression as MemberInitExpression);
                if (expression is MethodCallExpression) return await MethodCallExpressionNode.CreateAsync(this, expression as MethodCallExpression);
                if (expression is NewArrayExpression) return new NewArrayExpressionNode(this, expression as NewArrayExpression);
                if (expression is NewExpression) return new NewExpressionNode(this, expression as NewExpression);
                if (expression is ParameterExpression) return new ParameterExpressionNode(this, expression as ParameterExpression);
                if (expression is TypeBinaryExpression) return await TypeBinaryExpressionNode.CreateAsync(this, expression as TypeBinaryExpression);
                if (expression is UnaryExpression) return new UnaryExpressionNode(this, expression as UnaryExpression);

                throw new ArgumentException("Unknown expression of type " + expression.GetType());
            });
        }

        /// <summary>
        /// Creates an type node from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public TypeNode Create(Type type)
        {
            return new TypeNode(this, type);
        }
    }
}