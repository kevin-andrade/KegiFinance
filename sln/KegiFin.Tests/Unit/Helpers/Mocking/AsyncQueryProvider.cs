using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace KegiFin.Tests.Unit.Helpers.Mocking;

public class AsyncQueryProvider<TEntity>(IQueryProvider inner) : IAsyncQueryProvider
{
    public IQueryable CreateQuery(Expression expression)
        => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestAsyncEnumerable<TElement>(expression);

    public object? Execute(Expression expression)
        => inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
    {
        try
        {
            var expectedType = typeof(TResult);

            // If the expected result is a Task (for example, Task<Category>)
            if (expectedType.IsGenericType && expectedType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultType = expectedType.GenericTypeArguments[0]; // Ex: Category

                // Compile and execute the expression
                var compiled = Expression.Lambda(expression).Compile();
                var result = compiled.DynamicInvoke();

                // Creates the Task dynamically
                var taskResult = typeof(Task)
                    // ReSharper disable once NullableWarningSuppressionIsUsed
                    .GetMethod(nameof(Task.FromResult))!
                    .MakeGenericMethod(resultType)
                    .Invoke(null, [result]);

                // ReSharper disable once NullableWarningSuppressionIsUsed
                return (TResult)taskResult!;
            }

            // Otherwise, it runs normally.
            return inner.Execute<TResult>(expression);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error executing the LINQ expression in the mock: {expression}", ex);
        }
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        => Execute<TResult>(expression);

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        => new TestAsyncEnumerable<TResult>(expression);
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

    public TestAsyncEnumerable(Expression expression) : base(expression) { }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);
}
public class TestAsyncEnumerator<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    public ValueTask DisposeAsync()
    {
        inner.Dispose();
        return new ValueTask();
    }

    public ValueTask<bool> MoveNextAsync()
        => new ValueTask<bool>(inner.MoveNext());

    public T Current => inner.Current;
}
