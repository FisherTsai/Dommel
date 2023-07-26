using System;

using Xunit;

namespace Dommel.Tests;

public class SqlExpressionTests
{
    [Fact]
    public void ToString_ReturnsSql()
    {
        var sqlExpression = new SqlExpression<Product>(new SqlServerSqlBuilder());
        var sql = sqlExpression.Where(p => p.Name == "Chai").ToSql();
        Assert.Equal(" where ([Products].[FullName] = @p1)", sql);
        Assert.Equal(sql, sqlExpression.ToString());
    }

    [Fact]
    public void ToStringVisitation_ReturnsSql()
    {
        var sqlExpression = new SqlExpression<Product>(new SqlServerSqlBuilder());
        var sql = sqlExpression.Where(p => p.CategoryId.ToString() == "1").ToSql();
        Assert.Equal(" where (CAST([Products].[CategoryId] AS CHAR) = @p1)", sql);
        Assert.Equal(sql, sqlExpression.ToString());
    }

    [Fact]
    public void ToString_ThrowsWhenCalledWithArgument()
    {
        var sqlExpression = new SqlExpression<Product>(new SqlServerSqlBuilder());
        var ex = Assert.Throws<ArgumentException>(() => sqlExpression.Where(p => p.CategoryId.ToString("n2") == "1"));
        Assert.Contains("ToString-expression should not contain any argument.", ex.Message);
    }

    [Fact]
    public void ToStringParentheses_ReturnsSql()
    {
        var sqlExpression = new SqlExpression<Category>(new SqlServerSqlBuilder());
        var sql = sqlExpression.Where(p => p.Id > 0 && (p.Id == 1 || p.Id < 0)).ToSql();
        Assert.Equal(" where ([Categories].[Id] > @p1 and ([Categories].[Id] = @p2 or [Categories].[Id] < @p3))", sql);
        Assert.Equal(sql, sqlExpression.ToString());
    }
}