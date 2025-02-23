using System.Diagnostics.CodeAnalysis;

namespace Assassins.Web.Utils;

public class Result<TError>
{
	[MemberNotNullWhen(false, nameof(_error))]
	private bool IsSuccess { get; }

	private readonly TError? _error;

	protected Result(bool isSuccess, TError error)
	{
		IsSuccess = isSuccess;
		_error = error;
	}

	public static Result<TError> Success() => new(true, default!);
	public static Result<TError> Failure(TError error) => new(false, error);

	public T Match<T>(
		Func<T> onSuccess,
		Func<TError, T> onFailure)
	{
		return IsSuccess ? onSuccess() : onFailure(_error);
	}

	public async Task<T> MatchAsync<T>(
		Func<Task<T>> onSuccess,
		Func<TError, T> onFailure)
	{
		return IsSuccess ? await onSuccess() : onFailure(_error);
	}
}

public class Result<T, TError>
{
	[MemberNotNullWhen(true, nameof(_data))]
	[MemberNotNullWhen(false, nameof(_error))]
	private bool IsSuccess { get; }

	private readonly T? _data;
	private readonly TError? _error;

	protected Result(bool isSuccess, T? data, TError error)
	{
		IsSuccess = isSuccess;
		_data = data;
		_error = error;
	}

	public static Result<T, TError> Success(T data)
		=> new(true, data, default!);
	public static Result<T, TError> Failure(TError error)
		=> new(false, default, error);

	public TReturn Match<TReturn>(
		Func<T, TReturn> onSuccess,
		Func<TError, TReturn> onFailure)
	{
		return IsSuccess ? onSuccess(_data) : onFailure(_error);
	}


	public async Task<TReturn> MatchAsync<TReturn>(
		Func<T, Task<TReturn>> onSuccess,
		Func<TError, TReturn> onFailure)
	{
		return IsSuccess ? await onSuccess(_data) : onFailure(_error);
	}
}
