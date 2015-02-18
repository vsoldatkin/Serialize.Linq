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
    [DataContract(Name = "M")]
#endif
#if !SILVERLIGHT
    [Serializable]
#endif
    #endregion
    public class MemberExpressionNode : ExpressionNode<MemberExpression>
    {
        public MemberExpressionNode() { }

        public MemberExpressionNode(INodeFactory factory, MemberExpression expression)
            : base(factory, expression) { }

        protected MemberExpressionNode(INodeFactory factory, ExpressionType nodeType, Type type)
            : base(factory, nodeType, type) { }

        public static async Task<MemberExpressionNode> CreateAsync(INodeFactory factory, MemberExpression expression)
        {
            MemberExpressionNode result = new MemberExpressionNode(factory, expression.NodeType, expression.Type);

            await result.InitializeAsync(expression);

            return result;
        }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "E")]
#endif
        #endregion
        public ExpressionNode Expression { get; set; }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberInfoNode Member { get; set; }

        protected override void Initialize(MemberExpression expression)
        {
            this.Expression = this.Factory.Create(expression.Expression);
            this.Member = new MemberInfoNode(this.Factory, expression.Member);
        }

        protected override async Task InitializeAsync(MemberExpression expression)
        {
            this.Expression = await this.Factory.CreateAsync(expression.Expression);
            this.Member = new MemberInfoNode(this.Factory, expression.Member);
        }

        public override Expression ToExpression(ExpressionContext context)
        {
            var member = this.Member.ToMemberInfo(context);
            return System.Linq.Expressions.Expression.MakeMemberAccess(this.Expression != null ? this.Expression.ToExpression(context) : null, member);
        }
    }
}
