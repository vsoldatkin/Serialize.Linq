#region Copyright
//  Copyright, Sascha Kiefer (esskar)
//  Released under LGPL License.
//  
//  License: https://raw.github.com/esskar/Serialize.Linq/master/LICENSE
//  Contributing: https://github.com/esskar/Serialize.Linq
#endregion

using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "IF")]   
#endif
#if !SILVERLIGHT
    [Serializable]
#endif
    #endregion
    public class ConditionalExpressionNode : ExpressionNode<ConditionalExpression>
    {
        public ConditionalExpressionNode() { }

        public ConditionalExpressionNode(INodeFactory factory, ConditionalExpression expression)
            : base(factory, expression) { }

        protected ConditionalExpressionNode(INodeFactory factory, ExpressionType nodeType, Type type)
            : base(factory, nodeType, type) { }

        public static async Task<ConditionalExpressionNode> CreateAsync(INodeFactory factory, ConditionalExpression expression)
        {
            ConditionalExpressionNode result = new ConditionalExpressionNode(factory, expression.NodeType, expression.Type);

            await result.InitializeAsync(expression);

            return result;
        }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "IFF")]
#endif
        #endregion
        public ExpressionNode IfFalse { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "IFT")]
#endif
        #endregion
        public ExpressionNode IfTrue { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "C")]
#endif
        #endregion
        public ExpressionNode Test { get; set; }

        /// <summary>
        /// Initializes the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected override void Initialize(ConditionalExpression expression)
        {
            this.Test = this.Factory.Create(expression.Test);
            this.IfTrue = this.Factory.Create(expression.IfTrue);
            this.IfFalse = this.Factory.Create(expression.IfFalse);
        }

        protected override async Task InitializeAsync(ConditionalExpression expression)
        {
            this.Test = await this.Factory.CreateAsync(expression.Test);
            this.IfTrue = await this.Factory.CreateAsync(expression.IfTrue);
            this.IfFalse = await this.Factory.CreateAsync(expression.IfFalse);
        }

        /// <summary>
        /// Converts this instance to an expression.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Expression ToExpression(ExpressionContext context)
        {
            return Expression.Condition(this.Test.ToExpression(context), this.IfTrue.ToExpression(context), this.IfFalse.ToExpression(context));
        }
    }
}
